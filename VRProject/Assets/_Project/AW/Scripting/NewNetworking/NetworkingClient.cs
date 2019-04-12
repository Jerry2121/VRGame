using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{

    abstract class NetworkingClient : MonoBehaviour
    {
        public abstract int ClientID();

        public abstract void WriteMessage(string message);

        public abstract void Disconnect();
    }
}
