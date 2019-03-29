using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{

    [RequireComponent(typeof(NetworkSpawnable))]
    public class NetworkingPosition : MonoBehaviour
    {

        NetworkSpawnable netSpawn;

        void Start()
        {
            netSpawn = GetComponent<NetworkSpawnable>();
        }
        void Update()
        {
            NetworkingManager.Instance.SendMessage(NetworkTranslater.CreatePositionMessage(NetworkingManager.ClientID(), netSpawn.objectID, transform.position));
        }

    }

}