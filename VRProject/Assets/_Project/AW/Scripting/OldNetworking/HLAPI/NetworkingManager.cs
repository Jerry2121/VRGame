using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace VRGame.OldNetworking
{
#pragma warning disable CS0618 // Type or member is obsolete
    [System.Obsolete("This uses obsolete Unet code")]
    public sealed class NetworkingManager
    {
        #region singleton
        private static readonly Lazy<NetworkingManager> lazy =
            new Lazy<NetworkingManager>(() => new NetworkingManager());

        public static NetworkingManager Instance { get { return lazy.Value; } }
        #endregion

        private NetworkingManager()
        {
            Logger.Instance.Init();

            Debug.Log("Creating NetworkingManager Instance");

            networkManager = NetworkManager.singleton;
            if(networkManager == null)
                throw new Exception("The Networkmanager cannot be found!");

            networkingDiscovery = networkManager.GetComponent<NetworkingDiscovery>();
            if (networkingDiscovery == null)
                throw new Exception("The NetworkingDiscovery component not found on the Network Manager!");
            networkingDiscovery.Init(networkManager);
            initialized = true;
        }
        private const int MAX_CONNECTIONS = 2;

        public static bool initialized = false;

        private NetworkManager networkManager;
        private NetworkingDiscovery networkingDiscovery;

        public bool IsHost { get { return Instance.networkingDiscovery.isServer; } }

        public void JoinLANGame()
        {
            networkingDiscovery.Initialize();

            networkingDiscovery.StartAsClient();
            //StartCoroutine(lobbyManager.WaitForJoinLAN());
            Debug.Log("Joining LAN Game");
        }

        public void CreateLANGameAsHost()
        {
            networkingDiscovery.Initialize();

            networkManager.StartHost(null, MAX_CONNECTIONS);
            networkingDiscovery.StartAsServer();
            //StartCoroutine(lobbyManager.WaitForCreateLAN());
            Debug.Log("Creating LAN Game");
        }

        public void Disconnect()
        {
            networkManager.StopHost();
            networkingDiscovery.StopBroadcast();
            Debug.Log("Disconnecting");
        }

        /// <summary>
        /// Instantiates an object on the server, then tells the NetworkServer to spawn it on all clients. Will throw exception if called on clients
        /// </summary>
        /// <param name="_obj"></param>
        /// <param name="_position"></param>
        /// <param name="_rotation"></param>
        public GameObject InstantiateOverNetwork(GameObject _obj, Vector3 _position, Quaternion _rotation)
        {
            if (IsHost == false)
            {
                throw new Exception("InstantiateOverNetwork cannot be called on clients!");
            }

            GameObject GO = UnityEngine.Object.Instantiate(_obj, _position, _rotation);
            if (Instance.networkManager != null)
                NetworkServer.Spawn(GO);
            return GO;
        }


        #region Player Tracking

        private const string PLAYER_ID_PREFIX = "Player ";

        int playersAmount;

        private Dictionary<string, TempPlayer> players = new Dictionary<string, TempPlayer>();

        public void RegisterPlayer(string _netID, TempPlayer _player)
        {
            playersAmount++;
            string playerID = PLAYER_ID_PREFIX + _netID;
            players.Add(playerID, _player);
            _player.transform.name = playerID;
            Debug.Log("Player " + playerID + " has been registered");
        }

        public void UnregisterPlayer(string _playerID)
        {
            playersAmount--;
            players.Remove(_playerID);
            Debug.Log("Player with ID '" + _playerID + "' has been unregistered");
        }

        public TempPlayer GetPlayer(string _playerID)
        {
            if (players.ContainsKey(_playerID))
                return players[_playerID];
            else return null;
        }

        public TempPlayer[] GetAllPlayers()
        {
            return players.Values.ToArray();
        }

        public TempPlayer GetLocalPlayer()
        {
            foreach (TempPlayer player in players.Values)
            {
                if (player.isLocalPlayer)
                {
                    return player;
                }
            }
            return null;
        }

#if UNITY_EDITOR
        [MenuItem("VRGame/Debug/AddNullPlayer")]
        public static void AddNullPlayers()
        {
            Instance.players.Add(("null" + Instance.players.Count), null);
            Instance.playersAmount++;
        }
        [MenuItem("VRGame/InitNetworkManager")]
        public static void InitNetworkManager()
        {
            NetworkingManager nw = Instance;
        }
#endif

        #endregion
    }
#pragma warning restore CS0618 // Type or member is obsolete
}
