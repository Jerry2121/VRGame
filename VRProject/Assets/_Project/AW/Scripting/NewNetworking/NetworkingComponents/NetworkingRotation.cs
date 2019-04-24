using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace VRGame.Networking {

    [AddComponentMenu("VR Networking/Networking Rotation")]
    [DisallowMultipleComponent]
    public class NetworkingRotation : NetworkObjectComponent
    {
        quaternion lastSentRotation = quaternion.identity;

        public override int ID { get => m_ID; protected set => m_ID = value; }

        [HideInInspector]
        [SerializeField]
        int m_ID;

        public override NetworkObject networkObject { get; protected set; }

        // Start is called before the first frame update
        void Start()
        {
            networkObject = GetNetworkObjectForObject(this.transform);
            RegisterSelf();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //If the object is controlled by its local client, and this isn't an object local to us, return
            if (networkObject.LocalAuthority() && networkObject.isLocalObject() == false)
                return;

            if (transform.rotation != lastSentRotation)
            {
                lastSentRotation = transform.rotation;

                float4 roundedRot = new float4();

                roundedRot.x = (float)Math.Round(transform.rotation.x, 3);
                roundedRot.y = (float)Math.Round(transform.rotation.y, 3);
                roundedRot.z = (float)Math.Round(transform.rotation.z, 3);
                roundedRot.w = (float)Math.Round(transform.rotation.w, 3);

                NetworkingManager.Instance.SendNetworkMessage(NetworkTranslater.CreateRotationMessage(NetworkingManager.ClientID(), networkObject.m_ObjectID, ID, roundedRot));
            }
        }

        void RotateTo(float x, float y, float z, float w)
        {
            quaternion rotation = new quaternion(x, y, z, w);

            transform.rotation = rotation;
            lastSentRotation = rotation;
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