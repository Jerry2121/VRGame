﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace VRGame.Networking
{

    [DisallowMultipleComponent]
    public class NetworkingPosition : NetworkObjectComponent
    {

        NetworkObject netObject;
        
        Vector3 lastSentPosition = Vector3.zero;

        void Start()
        {
            netObject = GetComponent<NetworkObject>();
        }
        void FixedUpdate()
        {
            if (transform.position != lastSentPosition)
            {
                float3 roundedPos = new float3();

                roundedPos.x = (float)Math.Round(transform.position.x, 3);
                roundedPos.y = (float)Math.Round(transform.position.y, 3);
                roundedPos.z = (float)Math.Round(transform.position.z, 3);

                NetworkingManager.Instance.SendNetworkMessage(NetworkTranslater.CreatePositionMessage(NetworkingManager.ClientID(), netObject.objectID, roundedPos));
                lastSentPosition = transform.position;
            }

        }

        //public void RecieveMoveMessage(float xMov, float zMov)
        //{
        //    transform.Translate(xMov * 0.5f, 0, zMov * 0.5f);
        //}

        public void RecievePositionMessage(float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }

        public override void RecieveMessage(string recievedMessage)
        {
            if (NetworkTranslater.TranslatePositionMessage(recievedMessage, out int clientID, out int objectID, out float x, out float y, out float z) == false)
                return;

            RecievePositionMessage(x, y, z);

        }
    }

}