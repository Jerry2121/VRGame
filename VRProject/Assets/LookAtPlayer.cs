using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("LocalPlayer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player);
    }
}
