using System;
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
        Vector3 lastSentPosition = Vector3.zero;

        public override int ID { get => m_ID; protected set => m_ID = value; }

        [HideInInspector]
        [SerializeField]
        int m_ID;

        Rigidbody rb;

        public override NetworkObject networkObject { get; protected set; }

        void Start()
        {
            if (GetComponent<Rigidbody>() != null)
                rb = GetComponent<Rigidbody>();
            networkObject = GetNetworkObjectForObject(this.transform);
            RegisterSelf();
        }

        void FixedUpdate()
        {
            //If the object is controlled by its local client, and this isn't an object local to us, return
            if (networkObject.LocalAuthority() && networkObject.isLocalObject() == false)
            {
                if (rb != null)
                    rb.velocity = Vector3.zero;
                return;
            }

            if (transform.position != lastSentPosition)
            {
                lastSentPosition = transform.position;

                float3 roundedPos = new float3();

                roundedPos.x = (float)Math.Round(transform.position.x, 3);
                roundedPos.y = (float)Math.Round(transform.position.y, 3);
                roundedPos.z = (float)Math.Round(transform.position.z, 3);

                NetworkingManager.Instance.SendNetworkMessage(NetworkTranslater.CreatePositionMessage(NetworkingManager.ClientID(), networkObject.m_ObjectID, ID, roundedPos));
            }
        }

        //public void RecieveMoveMessage(float xMov, float zMov)
        //{
        //    transform.Translate(xMov * 0.5f, 0, zMov * 0.5f);
        //}

        public void MoveTo(float x, float y, float z)
        {
            float3 position = new float3(x, y, z);
            transform.position = position;
            lastSentPosition = position;
        }

        public override void RecieveMessage(string recievedMessage)
        {
            Debug.Log(" -- Recieved Pos Msg", this.gameObject);

            if (NetworkTranslater.TranslatePositionMessage(recievedMessage, out int clientID, out int objectID, out int componenentID, out float x, out float y, out float z) == false)
                return;

            MoveTo(x, y, z);

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