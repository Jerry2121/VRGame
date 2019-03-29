using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using VRGame.Networking;

public class Test2 : MonoBehaviour
{
    public GameObject spawn;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Spawn();
        }

    }
    
    void Spawn()
    {
        float foo = Random.Range(-10, -10);
        NetworkingManager.Instance.InstantiateOverNetwork(spawn.GetComponent<NetworkSpawnable>().objectName, foo, 0, foo);
    }

}

