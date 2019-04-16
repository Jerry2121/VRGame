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
        GameObject[] spawnableGameObjects;

        public static NetworkingManager Instance;

        public GameObject playerPrefab;

        public Dictionary<int, TempPlayer> playerDictionary = new Dictionary<int, TempPlayer>();
        public Dictionary<int, NetworkObject> networkedObjectDictionary = new Dictionary<int, NetworkObject>();

        [SerializeField]
        string m_NetworkAddress = "localhost";
        [SerializeField]
        int m_NetworkPort = 9000;

        [SerializeField] SceneReference offlineScene;
        [SerializeField] SceneReference onlineScene;

        bool useJobClient;
        bool useJobServer;

        [SerializeField] bool showGUI;
        [SerializeField] bool debug;
        [SerializeField] int offsetX;
        [SerializeField] int offsetY;

        NetworkingClient m_Client;

        NetworkServer m_Server;
        bool m_Connected;

        Dictionary<string, GameObject> spawnableObjectDictionary = new Dictionary<string, GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            if(Instance != null)
            {
                Debug.LogError("NetworkingManager -- Start: Instance was not equal to null! Destroying this component!");
                Destroy(this);
                return;
            }
            Instance = this;
            Logger.Instance.Init();
            DontDestroyOnLoad(Instance);
            SceneManager.sceneLoaded += OnSceneLoaded;

            foreach(var GO in spawnableGameObjects)
            {
                NetworkObject netSpawn = GO.GetComponent<NetworkObject>();
                if(netSpawn == null)
                {
                    Debug.LogError("NetworkingManager -- Start: Gameobject does not have a NetworkSpawnable component");
                    continue;
                }

                spawnableObjectDictionary.Add(netSpawn.objectName, GO);
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
            else if (spawnableObjectDictionary.ContainsKey(objectType))
            {
                tempGO = Instantiate(spawnableObjectDictionary[objectType], new Vector3(x,y,z), Quaternion.identity);
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
            netObj.objectID = objectID;

            if (networkedObjectDictionary.ContainsKey(objectID))
            {
                Debug.LogError(string.Format("The networkedObjectDictionary already has an entry for {0}! The objects type was {1}. Destroying the object" , objectID, objectType), tempGO);
                Destroy(tempGO);
                return;
            }
            networkedObjectDictionary.Add(objectID, netObj);

            if (clientID == ClientID())
                netObj.SetLocal();
        }

        GameObject InstantiatePlayer(int clientID, int objectID, string objectName, float x, float y, float z)
        {
            //If we have already set up the player, return
            if (playerDictionary.ContainsKey(clientID) && playerDictionary[clientID] != null)
                return null;

            if (playerDictionary.ContainsKey(clientID) == false)
            {
                playerDictionary.Add(clientID, null);
            }

            //if(ID%2 == 0)
            //    //Spawn player 1 prefab
            //    else
            //    //spawn player 2


            TempPlayer player = Instantiate(playerPrefab, new Vector3(x, y, z), Quaternion.identity).GetComponent<TempPlayer>();

            playerDictionary[clientID] = player;
            player.SetPlayerID(clientID);

            if (clientID == m_Client.ClientID()) //The message came from us, the local player
                player.SetIsLocalPlayer();

            return player.gameObject;
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
            if (useJobClient)
                m_Client = gameObject.AddComponent<NetworkClientJobs>();
            else
                m_Client = gameObject.AddComponent<NetworkClient>();

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- StartClient: Client created.");
        }

        void StartServer()
        {
            if(useJobServer)
                m_Server = gameObject.AddComponent<NetworkServer>();
            else
                m_Server = gameObject.AddComponent<NetworkServer>();

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- StartServer: Server created.");
        }

        private void Disconnect()
        {
            if (m_Server != null)
                StopHost();
            else
                ClientDisconnect();

            //Destroy all networked objects
            foreach (var netObject in networkedObjectDictionary.Keys)
            {
                Destroy(networkedObjectDictionary[netObject].gameObject);
            }
            networkedObjectDictionary.Clear();
            playerDictionary.Clear();
            SwitchToOfflineScene();
        }

        public void ClientDisconnect()
        {
            if (m_Client != null)
            {
                m_Client.Disconnect();
                Destroy(m_Client);
                m_Client = null;
            }

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- ClientDisconnect: Client disconnected.");
        }

        public void StopHost()
        {
            ClientDisconnect();

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
            SceneManager.LoadScene(offlineScene.Path);
        }

        public void SwitchToOnlineScene()
        {
            if(m_Client != null)
                SceneManager.LoadScene(onlineScene.Path);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            SendNetworkMessage(NetworkTranslater.CreateLoadedInMessage(ClientID()));
            if(scene.path == onlineScene.Path && m_Client != null)
            {
                InstantiateOverNetwork("Player", Vector3.zero);
            }
        }

        public IPAddress NetworkAddress()
        {
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

        public static int ClientID()
        {
            if(Instance.m_Client != null)
                return Instance.m_Client.ClientID();
            return -1;
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        void DebugCreateClient()
        {
            gameObject.AddComponent<NetworkClient>();
        }
#endif

    }
}