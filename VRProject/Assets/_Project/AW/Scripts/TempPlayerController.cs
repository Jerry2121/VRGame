using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkPlayer = VRGame.Networking.NetworkPlayer;

[RequireComponent(typeof(Rigidbody))]
public class TempPlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<NetworkPlayer>() != null && GetComponent<NetworkPlayer>().IsLocalPlayer() == false)
            return;

            float xMov = Input.GetAxisRaw("Horizontal");
        float yMov = Input.GetAxisRaw("Vertical");

        transform.Translate(xMov * moveSpeed, 0, yMov * moveSpeed);
    }
}
