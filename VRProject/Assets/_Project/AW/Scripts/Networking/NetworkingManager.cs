﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRGame.Networking
{

    public class NetworkingManager : MonoBehaviour
    {
        [SerializeField]
        GameObject[] m_SpawnableGameObjects;

        public static NetworkingManager s_Instance;

        public GameObject playerPrefab;

        public Dictionary<int, NetworkPlayer> m_PlayerDictionary = new Dictionary<int, NetworkPlayer>();
        public Dictionary<int, NetworkObject> m_NetworkedObjectDictionary = new Dictionary<int, NetworkObject>();

        [SerializeField]
        string m_NetworkAddress = "localhost";
        [SerializeField]
        int m_NetworkPort = 9000;

        [SerializeField] SceneReference m_OfflineScene;
        [SerializeField] SceneReference m_OnlineScene;

        [SerializeField] bool showGUI;
        [SerializeField] bool debug;
        [SerializeField] int offsetX;
        [SerializeField] int offsetY;

        NetworkClient m_Client;

        NetworkServer m_Server;
        bool m_Connected;

        Dictionary<string, GameObject> m_SpawnableObjectDictionary = new Dictionary<string, GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            if(s_Instance != null)
            {
                Debug.LogError("NetworkingManager -- Start: Instance was not equal to null! Destroying this component!");
                Destroy(this);
                return;
            }
            s_Instance = this;
            DontDestroyOnLoad(s_Instance);
            SceneManager.sceneLoaded += OnSceneLoaded;

            foreach(var GO in m_SpawnableGameObjects)
            {
                NetworkObject netSpawn = GO.GetComponent<NetworkObject>();
                if(netSpawn == null)
                {
                    Debug.LogError("NetworkingManager -- Start: Gameobject does not have a NetworkSpawnable component");
                    continue;
                }
                if(string.IsNullOrWhiteSpace(netSpawn.m_ObjectName))
                {
                    Debug.LogError("NetworkingManager -- Start: GameObject has no objectName on NetworkObject!");
                    continue;
                }
                m_SpawnableObjectDictionary.Add(netSpawn.m_ObjectName, GO);

                Application.runInBackground = true;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnGUI()
        {
            if (!showGUI)
                return;

            int xpos = 10 + offsetX;
            int ypos = 40 + offsetY;
            const int spacing = 24;

            if (m_Client == null && m_Server == null)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
                {
                    m_NetworkAddress = IPAddress.Loopback.ToString();
                    StartHost();
                }
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
                {
                    StartClient();
                }

                m_NetworkAddress = GUI.TextField(new Rect(xpos + 105, ypos, 95, 20), m_NetworkAddress);
                ypos += spacing;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
                {
                    StartServer();
                }
                ypos += spacing;
#endif
            }
            else
            {
                if(m_Server != null && m_Client == null)
                {
                    string warningMsg = "WARNING: server running with no client";
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.red;
                    GUI.Label(new Rect(xpos, ypos, 300, 20), warningMsg, style);
                    ypos += spacing;
                }
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disconnect(D)"))
                {
                    Disconnect();
                }
                ypos += spacing;

                if (m_Server != null)
                {
                    string serverMsg = "Server: address=" + m_Server.ServerIPAddress() + " port=" + m_NetworkPort;
                    GUI.Label(new Rect(xpos, ypos, 300, 20), serverMsg);
                    ypos += spacing;
                }
                else if (m_Client != null)
                {
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + m_NetworkAddress + " port=" + m_NetworkPort);
                    ypos += spacing;
                }
            }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (debug == false)
                return;

            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Testing Client"))
                {
                    DebugCreateClient();
                }
                ypos += spacing;
#endif

        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void SendNetworkMessage(string message)
        {
            if (m_Client == null)
                return;

            m_Client.WriteMessage(message);
        }

        public void RecieveInstantiateMessage(string recievedMessage)
        {
            if (NetworkTranslater.TranslateInstantiateMessage(recievedMessage, out int clientID, out int objectID, out string objectType, out float x, out float y, out float z) == false)
                return;

            if (objectID == -1) //The message does not have a valid objectID
                return;

            GameObject tempGO;

            Debug.Log(string.Format("Recieved message to instantiate a {0} from client {1}", objectType, clientID));

            //Do unique player stuff
            if (objectType == "Player")
            {
                tempGO = InstantiatePlayer(clientID, objectID, objectType, x, y, z);
                if (tempGO == null) return;
            }
            //otherwise see if it is a spawnable object
            else if (m_SpawnableObjectDictionary.ContainsKey(objectType))
            {
                tempGO = Instantiate(m_SpawnableObjectDictionary[objectType], new Vector3(x,y,z), Quaternion.identity);
            }
            //we couldn't spawn it. It likely is either not on the Manager, or doesn't have a NetworkObject component
            else
            {
                Debug.LogError(
                    "NetworkingManager -- RecieveInstantiateMessage: Cannot spawn " + objectType + " over the network. " +
                    "It either has not been added to the gamemanager, or it does not have a NetworkObject component.");
                return;
            }

            NetworkObject netObj = tempGO.GetComponent<NetworkObject>();
            netObj.m_ObjectID = objectID;

            if (m_NetworkedObjectDictionary.ContainsKey(objectID))
            {
                Debug.LogError(string.Format("The networkedObjectDictionary already has an entry for {0}! The objects type was {1}. Destroying the object" , objectID, objectType), tempGO);
                Destroy(tempGO);
                return;
            }
            m_NetworkedObjectDictionary.Add(objectID, netObj);

            if (clientID == ClientID())
                netObj.SetLocal();
        }

        GameObject InstantiatePlayer(int clientID, int objectID, string objectName, float x, float y, float z)
        {
            //If we have already set up the player, return
            if (m_PlayerDictionary.ContainsKey(clientID) && m_PlayerDictionary[clientID] != null)
                return null;

            if (m_PlayerDictionary.ContainsKey(clientID) == false)
            {
                m_PlayerDictionary.Add(clientID, null);
            }

            //if(ID%2 == 0)
            //    //Spawn player 1 prefab
            //    else
            //    //spawn player 2


            NetworkPlayer player = Instantiate(playerPrefab, new Vector3(x, y, z), Quaternion.identity).GetComponent<NetworkPlayer>();

            m_PlayerDictionary[clientID] = player;
            player.SetPlayerID(clientID);

            if (clientID == m_Client.ClientID()) //The message came from us, the local player
                player.SetIsLocalPlayer();

            return player.gameObject;
        }

        public void DestroyPlayer(int playerID)
        {
            if (playerID == ClientID())
            {
                Debug.LogError("NetworkManager -- DestroyPlayer: This should only be called for disconnected player");
                return;
            }

            if(m_PlayerDictionary.ContainsKey(playerID) == false)
            {
                Debug.LogError("NetworkingManager -- DestroyPlayer: Tried to destroy a player that is not in the dictionary");
                return;
            }
            
            GameObject player = m_PlayerDictionary[playerID].gameObject;
            m_PlayerDictionary.Remove(playerID);

            int objectID = player.GetComponent<NetworkObject>().m_ObjectID;

            if (m_NetworkedObjectDictionary.ContainsKey(objectID))
            {
                m_NetworkedObjectDictionary.Remove(objectID);
            }

            Destroy(player);
        }

        public void InstantiateOverNetwork(string objectName, float x, float y, float z)
        {
            if(m_Client != null)
                SendNetworkMessage(NetworkTranslater.CreateInstantiateMessage(m_Client.ClientID(), -1, objectName, x, y, z));
        }

        public void InstantiateOverNetwork(string objectName, Vector3 position)
        {
            InstantiateOverNetwork(objectName, position.x, position.y, position.z);
        }

        public void StartHost()
        {
            StartServer();
            StartClient();

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- StartHost: Host created.");
        }

        public void StartClient()
        {
                m_Client = gameObject.AddComponent<NetworkClient>();

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- StartClient: Client created.");
        }

        void StartServer()
        {
                m_Server = gameObject.AddComponent<NetworkServer>();

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- StartServer: Server created.");
        }

        public void Disconnect()
        {
            if (m_Server != null)
                StopHost();
            else
                StopClient();

            //Destroy all networked objects
            foreach (var netObject in m_NetworkedObjectDictionary.Keys)
            {
                Destroy(m_NetworkedObjectDictionary[netObject].gameObject);
            }
            m_NetworkedObjectDictionary.Clear();
            m_PlayerDictionary.Clear();
            SwitchToOfflineScene();
        }

        void StopClient()
        {
            if (m_Client != null)
            {
                m_Client.Disconnect();
                Destroy(m_Client, 1f);
                m_Client = null;
            }

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- ClientDisconnect: Client disconnected.");
        }

        void StopHost()
        {
            StopClient();

            if(m_Server != null)
            {
                Destroy(m_Server);
                m_Server = null;
            }

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- StopHost: Server stopped.");
        }

        public void SwitchToOfflineScene()
        {
            SceneManager.LoadScene(m_OfflineScene.Path);
        }

        public void SwitchToOnlineScene()
        {
            if(m_Client != null)
                SceneManager.LoadScene(m_OnlineScene.Path);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            SendNetworkMessage(NetworkTranslater.CreateLoadedInMessage(ClientID()));
            if(scene.path == m_OnlineScene.Path && m_Client != null)
            {
                SpawnSceneObjectsOverNetwork();
                InstantiateOverNetwork("Player", Vector3.zero);
            }
        }

        /// <summary>
        /// Respawns networked objects already in the scene so they get properly set up for networking
        /// </summary>
        void SpawnSceneObjectsOverNetwork()
        {
            NetworkObject[] netObjects = GameObject.FindObjectsOfType<NetworkObject>();

            foreach(var netObject in netObjects)
            {
                if(IsHost()) //If we are the host we'll spawn them over the network, otherwise we will just destroy them,
                    InstantiateOverNetwork(netObject.m_ObjectName, netObject.transform.position);
                Destroy(netObject.gameObject);
            }

        }

        public IPAddress NetworkAddress()
        {
#if UNITY_EDITOR
            if (DevIPs(out IPAddress devAddress))
                return devAddress;
#endif
            if (m_NetworkAddress == "localhost")
                return IPAddress.Loopback;
            else if (IPAddress.TryParse(m_NetworkAddress, out IPAddress address))
                return address;
            else return null;
        }

        public bool IsHost()
        {
            return m_Server != null && m_Client != null;
        }

        public bool IsServer()
        {
            return m_Server != null;
        }

        public bool IsConnected()
        {
            return m_Client != null;
        }

        public static int ClientID()
        {
            if(s_Instance.m_Client != null)
                return s_Instance.m_Client.ClientID();
            return -1;
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        void DebugCreateClient()
        {
            gameObject.AddComponent<NetworkClient>();
        }
#endif

#if UNITY_EDITOR
        bool DevIPs(out IPAddress devAddress)
        {
            devAddress = null;

            try
            {
                switch (m_NetworkAddress)
                {
                    case "20":
                        devAddress = IPAddress.Parse("10.47.1.149");
                        break;
                    case "21":
                        devAddress = IPAddress.Parse("10.47.1.42");
                        break;
                    case "22":
                        devAddress = IPAddress.Parse("10.47.1.142");
                        break;
                    case "23":
                        throw new System.NotImplementedException();
                    case "24":
                        throw new System.NotImplementedException();
                    case "25":
                        throw new System.NotImplementedException();
                    case "26":
                        devAddress = IPAddress.Parse("10.47.1.36");
                        break;
                    default:
                        return false;
                }
            }
            catch(System.Exception ex)
            {
                Debug.LogError(ex.Message);
                return false;
            }
            return true;
        }
#endif
    }
}