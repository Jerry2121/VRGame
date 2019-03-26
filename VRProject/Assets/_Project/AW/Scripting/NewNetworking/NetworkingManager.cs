using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace VRGame.Networking
{

    public class NetworkingManager : MonoBehaviour
    {

        public static NetworkingManager Instance;

        public GameObject playerPrefab;

        public Dictionary<int, TempPlayer> playerDictionary = new Dictionary<int, TempPlayer>();

        [SerializeField]
        string m_NetworkAddress = "localhost";
        public int m_NetworkPort = 9000;

        [SerializeField] bool showGUI;
        [SerializeField] int offsetX;
        [SerializeField] int offsetY;

        NetworkClient m_Client;
        NetworkServer m_Server;
        bool m_Connected;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            Logger.Instance.Init();
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

        public void StartHost()
        {
            m_Server = gameObject.AddComponent<NetworkServer>();
            m_Client = gameObject.AddComponent<NetworkClient>();
            GameObject playerGO = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            playerGO.GetComponent<TempPlayer>().client = m_Client;

            if (Debug.isDebugBuild)
                Debug.Log("NetworkingManager -- StartHost: Host created.");
        }

        public void StartClient()
        {
            m_Client = gameObject.AddComponent<NetworkClient>();
            GameObject playerGO = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            playerGO.GetComponent<TempPlayer>().client = m_Client;

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


    }
}