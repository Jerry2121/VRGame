﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace VRGame.Networking
{
    [AddComponentMenu("VR Networking/Networking Position")]
    [DisallowMultipleComponent]
    public class NetworkingPosition : NetworkObjectComponent
    {
        Vector3 m_LastSentPosition = Vector3.zero;

        bool localControl; //If the object is currently under local control
        bool sentPositionRecently;

        public override int ID { get => m_ID; protected set => m_ID = value; }

        [HideInNormalInspector]
        [SerializeField]
        int m_ID;

        Rigidbody m_RigidBody;
        bool m_WasKinematic;
        public override NetworkObject m_NetworkObject { get; protected set; }

        void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            if (m_RigidBody != null)
            {
                m_WasKinematic = m_RigidBody.isKinematic;
            }
            m_NetworkObject = GetNetworkObjectForObject(this.transform);
            RegisterSelf();
        }

        void FixedUpdate()
        {
            //If the object is controlled by its local client, and this isn't an object local to us, return
            if (((m_NetworkObject.LocalAuthority() && m_NetworkObject.isLocalObject() == false) || (m_NetworkObject.LocalAuthority() == false && m_NetworkObject.PlayerIsInteracting() == false)) && localControl == false)
            {
                //if (m_RigidBody != null)
                //m_RigidBody.velocity = Vector3.zero;
                return;
            }
            else if (transform.position != m_LastSentPosition)
            {
                if (localControl == false)
                    localControl = true;

                SendPosition();
            }
            else if (localControl && sentPositionRecently == false)
            {
                sentPositionRecently = true;
                SendPosition();
                StartCoroutine(ResetSentPositionRecently());
            }
        }

        void SendPosition()
        {
            if (m_RigidBody != null && m_NetworkObject.m_Grabbed == false)
            {
                m_RigidBody.isKinematic = m_WasKinematic;
            }

            m_LastSentPosition = transform.position;

            float3 roundedPos = new float3();

            roundedPos.x = (float)Math.Round(transform.position.x, 3);
            roundedPos.y = (float)Math.Round(transform.position.y, 3);
            roundedPos.z = (float)Math.Round(transform.position.z, 3);

                //if (Debug.isDebugBuild)
                //Debug.Log(string.Format("Object {0} is sending a position message", gameObject.name));

            SendNetworkMessage(NetworkTranslater.CreatePositionMessage(NetworkingManager.ClientID(), m_NetworkObject.m_ObjectID, ID, roundedPos));

        }

        public override void SendNetworkMessage(string messageToSend)
        {
            NetworkingManager.s_Instance.SendNetworkMessage(messageToSend);
        }

        public override void RecieveNetworkMessage(string recievedMessage)
        {
            //Debug.Log(" -- Recieved Pos Msg", this.gameObject);

            if (NetworkTranslater.TranslatePositionMessage(recievedMessage, out int clientID, out int objectID, out int componenentID, out float x, out float y, out float z) == false)
                return;

            localControl = false;

            if (m_RigidBody != null)
                m_RigidBody.isKinematic = true;

            MoveTo(x, y, z);

        }

        public void MoveTo(float x, float y, float z)
        {
            float3 position = new float3(x, y, z);
            //transform.Translate((Vector3)position - transform.position, Space.World);
            transform.position = position;
            m_LastSentPosition = position;
        }

        IEnumerator ResetSentPositionRecently()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            sentPositionRecently = false;
        }

        public override void RegisterSelf()
        {
            base.RegisterSelf();
        }

        public override void SetID(int newID)
        {
            if (newID > -1)
            {
                if(Debug.isDebugBuild)
                    Debug.Log(string.Format("NetworkPosition -- SetID: ID set to {0}", newID));
                ID = newID;
            }
        }

        [ExecuteAlways]
        public override void SetNetworkObject(NetworkObject newNetworkObject)
        {
            base.SetNetworkObject(newNetworkObject);
        }

        public override void Reset()
        {
            base.Reset();
        }
    }

}