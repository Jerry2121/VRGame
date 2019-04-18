using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{

    public abstract class NetworkObjectChildComponent : MonoBehaviour
    {
        public abstract int ID { get; }

        public abstract void RegisterSelf();

        public abstract void RecieveMessage(string recievedMessage);

        public abstract void Reset();
    }
}