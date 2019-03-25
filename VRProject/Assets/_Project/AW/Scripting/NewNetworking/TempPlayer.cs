using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{
    [RequireComponent(typeof(Rigidbody))]
    public class TempPlayer : MonoBehaviour
    {

        Rigidbody rb;
        [SerializeField]
        public NetworkClient client;

        // Start is called before the first frame update
        void Start()
        {
            if (client != null)
                client.AssignPlayer(this);
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (client == null)
                return;

            float xMov = Input.GetAxisRaw("Horizontal");
            float zMov = Input.GetAxisRaw("Vertical");

            transform.Translate(xMov * 0.5f, 0, zMov * 0.5f);

            if(xMov != 0 && zMov != 0)
            {
                //string message = "1|Move|" + xMov.ToString() + "|" + zMov.ToString();
                client.SendMessage(NetworkTranslater.CreateMoveMessage(client.PlayerID, xMov, zMov));
            }
        }

        public void RecieveMoveMessage(float xMov, float zMov)
        {
            transform.Translate(xMov * 0.5f, 0, zMov * 0.5f);
        }

    }
}
