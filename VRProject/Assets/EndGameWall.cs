using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;

public class EndGameWall : MonoBehaviour
{
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            if (NetworkingManager.GetLocalPlayer() != null)
            {
                Player = NetworkingManager.GetLocalPlayer();
            }
            else
            {
                return;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Player)
        {
            this.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
