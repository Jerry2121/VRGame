using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{
    public abstract class NetworkObjectComponent : MonoBehaviour
    {
        public abstract int ID { get; }

        public abstract void RegisterSelf();

        public abstract void RecieveMessage(string recievedMessage);

        public abstract void SetID(int ID);

        public virtual void Reset()
        {
            if(CheckParentsForNetworkObject(this.transform) == false)
            {
                Debug.LogError("NetworkObjectComponet -- No Parents of this object have a NetworkObject component!", this);
            }
        }

        static bool CheckParentsForNetworkObject(Transform obj)
        {
            Transform parent = obj.transform.parent;

            if (parent == null)
                return false;

            if (parent.GetComponent<NetworkObject>() != null)
                return true;

            return CheckParentsForNetworkObject(parent);
        }

    }
}
