using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{
    public abstract class NetworkObjectComponent : MonoBehaviour, INetworkMessageReciever
    {

        public abstract NetworkObject m_NetworkObject { get; protected set; }
        public abstract int ID { get; protected set; }

        public virtual void RegisterSelf()
        {
            //Debug.Log(string.Format("Registering for ID {0} insID is {1}", ID, this.GetInstanceID()));
            if (m_NetworkObject.RegisterNetComponent(ID, this) == false)
                Debug.Log(string.Format("Failed to register for ID {0}", ID), this.gameObject);
        }

        public abstract void RecieveNetworkMessage(string recievedMessage);

        public abstract void SendNetworkMessage(string messageToSend);

        public abstract void SetID(int newID);

        [ExecuteAlways]
        public virtual void SetNetworkObject(NetworkObject newNetworkObject)
        {
            m_NetworkObject = newNetworkObject;
        }

        public virtual void Reset()
        {
            if(CheckObjectForNetworkObject(this.transform) == false)
            {
                Debug.LogError("NetworkObjectComponent -- This object and its parents do not have a NetworkObject component!", this);
            }
        }

        protected static bool CheckObjectForNetworkObject(Transform obj)
        {
            if (obj == null)
                return false;

            if (obj.GetComponent<NetworkObject>() != null)
                return true;

            return CheckObjectForNetworkObject(obj.transform.parent);
        }

        public static NetworkObject GetNetworkObjectForObject(Transform obj)
        {
            if (obj == null)
                return null;

            if (obj.GetComponent<NetworkObject>() != null)
                return obj.GetComponent<NetworkObject>();

            return GetNetworkObjectForObject(obj.transform.parent);
        }

    }
}
