using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{

    public class NetworkingPosition : NetworkObjectComponent
    {

        NetworkObject netObject;

        void Start()
        {
            netObject = GetComponent<NetworkObject>();
        }
        void Update()
        {
            NetworkingManager.Instance.SendNetworkMessage(NetworkTranslater.CreatePositionMessage(NetworkingManager.ClientID(), netObject.objectID, transform.position));
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