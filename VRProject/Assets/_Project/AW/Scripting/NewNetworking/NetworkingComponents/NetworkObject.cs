﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        Dictionary<int, NetworkObjectComponent> m_NetComponents = new Dictionary<int, NetworkObjectComponent>();


        public bool RegisterNetComponent(int ID, NetworkObjectComponent component)
        {
            if (m_NetComponents.ContainsKey(ID))
            {
                Debug.LogError(string.Format("There is already a registered component for ID {0}", ID.ToString()));
                return false;
            }

            m_NetComponents.Add(ID, component);
            return true;
        }

        public void UnRegisterNetComponent(int ID, NetworkObjectComponent component)
        {
            if (m_NetComponents.ContainsKey(ID) == false || m_NetComponents.ContainsValue(component) == false)
            {
                return;
            }

            m_NetComponents.Remove(ID);
            
        }
        
        public void RecieveMessage(string recievedMessage, int componentID)
        {
            if (m_NetComponents.ContainsKey(componentID) == false)
            {
                Debug.LogError("NetworkObject -- RecieveMessage: Recieved a message for an ID not in the dictionary");
                return;
            }

            m_NetComponents[componentID].RecieveMessage(recievedMessage);
        }

        [ExecuteInEditMode]
        public unsafe void CheckForChildrenNetworkComponentsRecursively(GameObject obj, int* ID)
        {
            Debug.Log("Checking" + obj.name);
            if (obj == null)
            {
                return;
            }

            if (obj.GetComponent<NetworkObject>() != null && obj.GetComponent<NetworkObject>() != this)
                Debug.LogError("Network Object -- Children should not have Network Object components!", obj);

            foreach (var netObjComp in obj.GetComponents<NetworkObjectComponent>())
            {
                if (netObjComp != null)
                {
                    netObjComp.SetID(*ID);
                    *ID = *ID + 1;
                }
            }

            foreach(Transform child in obj.transform) {
                if (child == null)
                    continue;

                CheckForChildrenNetworkComponentsRecursively(child.gameObject, ID);
            }

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
