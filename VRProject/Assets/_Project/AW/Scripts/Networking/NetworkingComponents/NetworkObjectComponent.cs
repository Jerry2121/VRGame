﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{
    public abstract class NetworkObjectComponent : MonoBehaviour
    {

        public abstract NetworkObject m_NetworkObject { get; protected set; }
        public abstract int ID { get; protected set; }

        public virtual void RegisterSelf()
        {
            //Debug.Log(string.Format("Registering for ID {0} insID is {1}", ID, this.GetInstanceID()));
            if (m_NetworkObject.RegisterNetComponent(ID, this) == false)
                Debug.Log(string.Format("Failed to register for ID {0}", ID), this.gameObject);
        }

        public abstract void RecieveMessage(string recievedMessage);

        public abstract void SetID(int ID);

        [ExecuteAlways]
        public virtual void SetNetworkObject(NetworkObject newNetworkObject)
        {
            m_NetworkObject = newNetworkObject;
        }

        public virtual void Reset()
        {
            if(CheckParentsForNetworkObject(this.transform) == false)
            {
                Debug.LogError("NetworkObjectComponent -- No Parents of this object have a NetworkObject component!", this);
            }
        }

        protected static bool CheckParentsForNetworkObject(Transform obj)
        {
            Transform parent = obj.transform.parent;

            if (parent == null)
                return false;

            if (parent.GetComponent<NetworkObject>() != null)
                return true;

            return CheckParentsForNetworkObject(parent);
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
