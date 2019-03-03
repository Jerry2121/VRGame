using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0618 // Type or member is obsolete
public class NetworkingManager
{
    #region singleton
    private static readonly Lazy<NetworkingManager> lazy =
        new Lazy<NetworkingManager>(() => new NetworkingManager());

    public static NetworkingManager Instance { get { return lazy.Value; } }
    #endregion

    private NetworkingManager()
    {
        networkManager = NetworkManager.singleton;
        networkingDiscovery = networkManager.GetComponent<NetworkingDiscovery>();
        networkingDiscovery.Init(networkManager);
    }
    private const int MAX_CONNECTIONS = 2;

    private NetworkManager networkManager;
    private NetworkingDiscovery networkingDiscovery;

    public static bool IsHost { get { return Instance.networkingDiscovery.isServer; } }

    public static void JoinLANGame()
    {
        Instance.networkingDiscovery.StartAsClient();
        //StartCoroutine(lobbyManager.WaitForJoinLAN());
    }

    public static void CreateLANGameAsHost()
    {
        Instance.networkManager.StartHost(null, MAX_CONNECTIONS);
        Instance.networkingDiscovery.StartAsServer();
        //StartCoroutine(lobbyManager.WaitForCreateLAN());
    }

    public static void Disconnect()
    {
        Instance.networkManager.StopHost();
        Instance.networkingDiscovery.StopBroadcast();
    }

    /// <summary>
    /// Instantiates an object on the server, then tells the NetworkServer to spawn it on all clients. Will throw exception if called on clients
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="_position"></param>
    /// <param name="_rotation"></param>
    public static GameObject InstantiateOverNetwork(GameObject _obj, Vector3 _position, Quaternion _rotation)
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
    /*
    private const string PLAYER_ID_PREFIX = "Player ";

    static int playersAmount;

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _player)
    {
        Instance.playersAmount++;
        string playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(playerID, _player);
        _player.transform.name = playerID;
    }

    public static void UnregisterPlayer(string _playerID)
    {
        Instance.playersAmount--;
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        if (players.ContainsKey(_playerID))
            return players[_playerID];
        else return null;
    }

    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

    public static Player GetLocalPlayer()
    {
        foreach (Player player in players.Values)
        {
            if (player.isLocalPlayer)
            {
                return player;
            }
        }
        return null;
    }

#if UNITY_EDITOR
    [MenuItem("VRGame/AddNullPlayer")]
    public static void AddNullPlayers()
    {
        players.Add(("null" + players.Count), null);
        Instance.playersAmount++;
    }
#endif
    */
    #endregion
}
#pragma warning restore CS0618 // Type or member is obsolete
