using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking {

    [AddComponentMenu("VR Networking/Network Object")]
    [DisallowMultipleComponent]
    public class NetworkObject : MonoBehaviour
    {
        [Header("The Objects Type")]
        public string objectName;

        public int objectID;

        [SerializeField] bool serverAuthority;

        bool m_isLocalObject;

        Dictionary<NetworkMessageContent, NetworkObjectComponent> netComponents = new Dictionary<NetworkMessageContent, NetworkObjectComponent>();


        public bool RegisterNetComponent(NetworkMessageContent contentType, NetworkObjectComponent component)
        {
            if (netComponents.ContainsKey(contentType))
            {
                Debug.LogError(string.Format("There is already a registered component handleing {0} messages", contentType.ToString()));
                return false;
            }

            netComponents.Add(contentType, component);
            return true;
        }

        public void UnRegisterNetComponent(NetworkMessageContent contentType, NetworkObjectComponent component)
        {
            if (netComponents.ContainsKey(contentType) == false || netComponents.ContainsValue(component) == false)
            {
                return;
            }

            netComponents.Remove(contentType);
            
        }
        
        public void RecieveMessage(string recievedMessage, NetworkMessageContent contentType)
        {
            if (netComponents.ContainsKey(contentType) == false)
                return;

            netComponents[contentType].RecieveMessage(recievedMessage);
        }

        public void SetLocal()
        {
            m_isLocalObject = true;
        }

        public bool isLocalObject()
        {
            return m_isLocalObject;
        }

    }

}
