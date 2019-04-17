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

        NetworkObject netObject;

        quaternion lastSentRotation = quaternion.identity;

        // Start is called before the first frame update
        void Start()
        {
            netObject = GetComponent<NetworkObject>();
            RegisterSelf();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //If the object is controlled by its local client, and this isn't an object local to us, return
            if (netObject.LocalAuthority() && netObject.isLocalObject() == false)
                return;

            if (transform.rotation != lastSentRotation)
            {
                lastSentRotation = transform.rotation;

                float4 roundedRot = new float4();

                roundedRot.x = (float)Math.Round(transform.rotation.x, 3);
                roundedRot.y = (float)Math.Round(transform.rotation.y, 3);
                roundedRot.z = (float)Math.Round(transform.rotation.z, 3);
                roundedRot.w = (float)Math.Round(transform.rotation.w, 3);

                NetworkingManager.Instance.SendNetworkMessage(NetworkTranslater.CreateRotationMessage(NetworkingManager.ClientID(), netObject.m_ObjectID, roundedRot));
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
            Debug.Log(" -- Recieved Rot Msg", this.gameObject);

            if (NetworkTranslater.TranslateRotationMessage(recievedMessage, out int clientID, out int objectID, out float x, out float y, out float z, out float w) == false)
                return;

            RotateTo(x, y, z, w);
        }

        public override void RegisterSelf()
        {
            netObject.RegisterNetComponent(NetworkMessageContent.Rotation, this);
        }

    }
}