using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;
using NetworkPlayer = VRGame.Networking.NetworkPlayer;

public class TestMoveCubes : MonoBehaviour
{
    [SerializeField]
    bool moveLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<NetworkPlayer>() != null && other.GetComponent<NetworkPlayer>().IsLocalPlayer())
            MoveCube.Move(true, moveLeft);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<NetworkPlayer>() != null && other.GetComponent<NetworkPlayer>().IsLocalPlayer())
            MoveCube.Move(false, moveLeft);
    }

}
