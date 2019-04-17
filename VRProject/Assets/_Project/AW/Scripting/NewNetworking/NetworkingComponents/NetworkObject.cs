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
        public string m_ObjectName;

        public int m_ObjectID;

        [SerializeField] bool m_LocalAuthority;
        [SerializeField] bool m_ServerAuthority;

        bool m_IsLocalObject;

        Dictionary<NetworkMessageContent, NetworkObjectComponent> m_NetComponents = new Dictionary<NetworkMessageContent, NetworkObjectComponent>();


        public bool RegisterNetComponent(NetworkMessageContent contentType, NetworkObjectComponent component)
        {
            if (m_NetComponents.ContainsKey(contentType))
            {
                Debug.LogError(string.Format("There is already a registered component handleing {0} messages", contentType.ToString()));
                return false;
            }

            m_NetComponents.Add(contentType, component);
            return true;
        }

        public void UnRegisterNetComponent(NetworkMessageContent contentType, NetworkObjectComponent component)
        {
            if (m_NetComponents.ContainsKey(contentType) == false || m_NetComponents.ContainsValue(component) == false)
            {
                return;
            }

            m_NetComponents.Remove(contentType);
            
        }
        
        public void RecieveMessage(string recievedMessage, NetworkMessageContent contentType)
        {
            if (m_NetComponents.ContainsKey(contentType) == false)
                return;

            m_NetComponents[contentType].RecieveMessage(recievedMessage);
        }

        public void SetLocal()
        {
            m_IsLocalObject = true;
        }

        public bool isLocalObject()
        {
            return m_IsLocalObject;
        }

        public bool LocalAuthority()
        {
            return m_LocalAuthority;
        }

        public bool ServerAuthority()
        {
            return m_ServerAuthority;
        }

    }

}
