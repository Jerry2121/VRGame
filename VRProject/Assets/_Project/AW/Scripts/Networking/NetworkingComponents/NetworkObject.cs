using System;
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
        bool m_PlayerInteracting;

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
        public unsafe void CheckForNetworkComponents(GameObject obj, int* ID)
        {
            //Debug.Log("Checking" + obj.name);
            if (obj == null)
            {
                return;
            }

            foreach (var netObjComp in obj.GetComponents<NetworkObjectComponent>())
            {
                if (netObjComp != null)
                {
                    netObjComp.SetID(*ID);
                    *ID = *ID + 1;
                    netObjComp.SetNetworkObject(this);
                }
            }

            *ID = 10;

            foreach (Transform child in obj.transform) {
                if (child == null)
                    continue;

                CheckForNetworkComponentsInChildrenRecursively(child.gameObject, ID);
            }

        }

        [ExecuteInEditMode]
        public unsafe void CheckForNetworkComponentsInChildrenRecursively(GameObject obj, int* ID)
        {
            //Debug.Log("Checking" + obj.name);
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
                    netObjComp.SetNetworkObject(this);
                }
            }

            foreach (Transform child in obj.transform)
            {
                if (child == null)
                    continue;

                CheckForNetworkComponentsInChildrenRecursively(child.gameObject, ID);
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

        public bool PlayerIsInteracting()
        {
            return m_PlayerInteracting;
        }

        public void SetPlayerInteracting(bool playerInteracting)
        {
            m_PlayerInteracting = playerInteracting;
        }

        public bool LocalAuthority()
        {
            return m_LocalAuthority;
        }

        public bool LocalObjectWithAuthority()
        {
            return m_LocalAuthority && m_IsLocalObject;
        }

        public bool ServerAuthority()
        {
            return m_ServerAuthority;
        }

        //private unsafe void OnValidate()
        //{
        //    Debug.Log("NetworkObject -- OnValidate: Checking For Network Components and setting IDs");
        //    int ID = 0;
        //    CheckForNetworkComponents(this.gameObject, &ID);
        //}

    }

}
