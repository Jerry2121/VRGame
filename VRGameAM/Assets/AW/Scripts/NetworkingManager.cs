using System;
using System.Collections;
using System.Collections.Generic;
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

    public void JoinLANGame()
    {
        networkingDiscovery.StartAsClient();
        //StartCoroutine(lobbyManager.WaitForJoinLAN());
    }

    public void CreateLANGameAsHost()
    {
        networkManager.StartHost(null, MAX_CONNECTIONS);
        networkingDiscovery.StartAsServer();
        //StartCoroutine(lobbyManager.WaitForCreateLAN());
    }

    public void Disconnect()
    {
        networkManager.StopHost();
        networkingDiscovery.StopBroadcast();
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

}
#pragma warning restore CS0618 // Type or member is obsolete
