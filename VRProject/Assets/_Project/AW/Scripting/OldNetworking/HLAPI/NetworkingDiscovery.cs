using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace VRGame.OldNetworking
{
#pragma warning disable CS0618 // Type or member is obsolete
    public sealed class NetworkingDiscovery : NetworkDiscovery
    {
        NetworkManager networkManager;

        public void Init(NetworkManager _netManager)
        {
            networkManager = _netManager;
            Initialize();
        }

        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            networkManager.networkAddress = fromAddress;
            //See if it is already connected, or else it will try to start and make a new player multipule times
            if (networkManager.IsClientConnected() == false)
            {
                networkManager.StartClient();
            }
        }

    }
#pragma warning restore CS0618 // Type or member is obsolete
}
