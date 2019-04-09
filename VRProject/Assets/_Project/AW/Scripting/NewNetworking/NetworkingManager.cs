using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

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

        [SerializeField] bool showGUI;
        [SerializeField] int offsetX;
        [SerializeField] int offsetY;

        NetworkClient m_Client;
        //public NetworkClient Client { get { return m_Client; }} 

        NetworkServer m_Server;
        bool m_Connected;

        Dictionary<string, GameObject> spawnableObjectDictionary = new Dictionary<string, GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            Logger.Instance.Init();

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
                    StartHost();
                }
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
                {
                    StartClient();
                }

                m_NetworkAddress = GUI.TextField(new Rect(xpos + 105, ypos, 95, 20), m_NetworkAddress);
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
                {
                    //manager.StartServer();
                }
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
                    string serverMsg = "Server: port=" + m_NetworkPort;
                    GUI.Label(new Rect(xpos, ypos, 300, 20), serverMsg);
                    ypos += spacing;
                }
                if (m_Client != null)
                {
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + m_NetworkAddress + " port=" + m_NetworkPort);
                    ypos += spacing;
                }
            }
        }


        public void SendNetworkMessage(string message)
        {
            if (m_Client == null)
                return;

            m_Client.WriteMessage(message);
        }

        public void RecieveInstantiateMessage(string recievedMessage)
        {
            if (NetworkTranslater.TranslateInstantiateMessage(recievedMessage, out int clientID, out int objectID, out string objectName, out float x, out float y, out float z) == false)
                return;

            if (objectID == -1) //The message does not have a valid objectID
                return;

            if (objectName == "Player")
            {
                InstantiatePlayer(clientID, objectID, objectName, x, y, z);
                return;
            }

            if (spawnableObjectDictionary.ContainsKey(objectName) == false)
            {
                Debug.LogError(
                    "NetworkingManager -- RecieveInstantiateMessage: Cannot spawn " + objectName + " over the network. " +
                    "It either has not been added to the gamemanager, or it does not have a NetworkObject component.");
                return;
            }

            GameObject temp = Instantiate(spawnableObjectDictionary[objectName]);
            temp.transform.position = new Vector3(x, y, z);
            temp.GetComponent<NetworkObject>().objectID = objectID;
            networkedObjectDictionary.Add(objectID, temp.GetComponent<NetworkObject>());
        }

        void InstantiatePlayer(int ID, int objectID, string objectName, float x, float y, float z)
        {
            Debug.LogError("A");
            //If we have already set up the player, return
            if (playerDictionary.ContainsKey(ID) && playerDictionary[ID] != null)
                return;

            Debug.LogError("B");

            if (playerDictionary.ContainsKey(ID) == false)
            {
                playerDictionary.Add(ID, null);
            }

            Debug.LogError("C");

            //if(ID%2 == 0)
            //    //Spawn player 1 prefab
            //    else
            //    //spawn player 2


            TempPlayer player = Instantiate(playerPrefab, new Vector3(x, y, z), Quaternion.identity).GetComponent<TempPlayer>();

            player.GetComponent<NetworkObject>().objectID = objectID;
            networkedObjectDictionary.Add(objectID, player.GetComponent<NetworkObject>());

            playerDictionary[ID] = player;

            player.SetPlayerID(ID);

            if (ID == m_Client.ClientID) //The message came from us, the local player
                player.SetIsLocalPlayer();
        }

        public void InstantiateOverNetwork(string objectName, float x, float y, float z)
        {
            SendNetworkMessage(NetworkTranslater.CreateInstantiateMessage(m_Client.ClientID, -1, objectName, x, y, z));
        }

        public void InstantiateOverNetwork(string objectName, Vector3 position)
        {
            InstantiateOverNetwork(objectName, position.x, position.y, position.z);
        }

        public void StartHost()
        {
            m_Server = gameObject.AddComponent<NetworkServer>();
            m_Client = gameObject.AddComponent<NetworkClient>();

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- StartHost: Host created.");
        }

        public void StartClient()
        {
            m_Client = gameObject.AddComponent<NetworkClient>();

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- StartClient: Client created.");
        }

        private void Disconnect()
        {
            if (m_Server != null)
                StopHost();
            else
                ClientDisconnect();
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

            //Destroy all networked objects
            foreach(var netObject in networkedObjectDictionary.Keys)
            {
                Destroy(networkedObjectDictionary[netObject]);
                networkedObjectDictionary.Remove(netObject);
            }
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
            return Instance.m_Client.ClientID;
        }

    }
}