using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{
    [RequireComponent(typeof(NetworkObject))]
    [DisallowMultipleComponent]
    public abstract class NetworkObjectComponent : MonoBehaviour
    {

        public abstract void RecieveMessage(string recievedMessage);

    }
}
