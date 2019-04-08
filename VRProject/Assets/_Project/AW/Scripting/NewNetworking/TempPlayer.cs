using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{
    [RequireComponent(typeof(Rigidbody))]
    public class TempPlayer : NetworkingBehavior
    {

        Rigidbody rb;
        //[SerializeField]
        //public NetworkClient client;

        bool m_IsLocalPlayer;
        int m_PlayerID;

        // Start is called before the first frame update
        void Start()
        {
            //if (client != null)
                //client.AssignPlayer(this);
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_IsLocalPlayer == false)
                return;

            float xMov = Input.GetAxisRaw("Horizontal");
            float zMov = Input.GetAxisRaw("Vertical");

            transform.Translate(xMov * 0.5f, 0, zMov * 0.5f);

            if(xMov != 0 && zMov != 0)
            {
                //client.WriteMessage(NetworkTranslater.CreateMoveMessage(client.PlayerID, xMov, zMov));
            }

            //NetworkingManager.Instance.SendNetworkMessage(NetworkTranslater.CreatePositionMessage(m_PlayerID, m_PlayerID, transform.position));
        }

        public void SetIsLocalPlayer()
        {
            if (m_IsLocalPlayer)
                Debug.LogWarning("TempPlayer -- SetIsLocalPlayer: isLocalPlayer already set! This should not be called on a setup player!");
            m_IsLocalPlayer = true;
        }

        public void SetPlayerID(int playerID)
        {
            m_PlayerID = playerID;
        }

        public void RecieveMoveMessage(float xMov, float zMov)
        {
            transform.Translate(xMov * 0.5f, 0, zMov * 0.5f);
        }

        public void RecievePositionMessage(float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }

    }
}
