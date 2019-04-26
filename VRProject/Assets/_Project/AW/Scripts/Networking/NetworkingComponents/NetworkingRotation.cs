﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace VRGame.Networking {

    [AddComponentMenu("VR Networking/Networking Rotation")]
    [DisallowMultipleComponent]
    public class NetworkingRotation : NetworkObjectComponent
    {
        quaternion m_LastSentRotation = quaternion.identity;

        public override int ID { get => m_ID; protected set => m_ID = value; }

        [HideInInspector]
        [SerializeField]
        int m_ID;

        public override NetworkObject m_NetworkObject { get; protected set; }

        // Start is called before the first frame update
        void Start()
        {
            m_NetworkObject = GetNetworkObjectForObject(this.transform);
            RegisterSelf();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //If the object is controlled by its local client, and this isn't an object local to us, return
            if (m_NetworkObject.LocalAuthority() && m_NetworkObject.isLocalObject() == false)
                return;

            if (transform.rotation != m_LastSentRotation)
            {
                m_LastSentRotation = transform.rotation;

                float4 roundedRot = new float4();

                roundedRot.x = (float)Math.Round(transform.rotation.x, 3);
                roundedRot.y = (float)Math.Round(transform.rotation.y, 3);
                roundedRot.z = (float)Math.Round(transform.rotation.z, 3);
                roundedRot.w = (float)Math.Round(transform.rotation.w, 3);

                NetworkingManager.s_Instance.SendNetworkMessage(NetworkTranslater.CreateRotationMessage(NetworkingManager.ClientID(), m_NetworkObject.m_ObjectID, ID, roundedRot));
            }
        }

        void RotateTo(float x, float y, float z, float w)
        {
            quaternion rotation = new quaternion(x, y, z, w);

            transform.rotation = rotation;
            m_LastSentRotation = rotation;
        }

        public override void RecieveMessage(string recievedMessage)
        {
            //Debug.Log(" -- Recieved Rot Msg", this.gameObject);

            if (NetworkTranslater.TranslateRotationMessage(recievedMessage, out int clientID, out int objectID, out int componentID, out float x, out float y, out float z, out float w) == false)
                return;

            RotateTo(x, y, z, w);
        }

        public override void RegisterSelf()
        {
            base.RegisterSelf();
        }
        public override void SetID(int newID)
        {
            if (newID > -1)
            {
                if (Debug.isDebugBuild)
                    Debug.Log(string.Format("NetworkRotation -- SetID: ID set to {0}", newID));
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