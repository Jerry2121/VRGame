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
        string[] foo = { "0|0|Pos|4|5", "1|1|Pos|3|5" };

        foreach (var val in foo)
        {
            Debug.Log(val);
        }

        string comb = NetworkTranslater.CombineMessages(foo);

        foo = NetworkTranslater.SplitMessages(comb);

        foreach(var val in foo)
        {
            Debug.Log(val);
        }

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
        float foo = Random.Range(-10, 10);
        float boo = Random.Range(-10, 10);
        NetworkingManager.Instance.InstantiateOverNetwork(spawn.GetComponent<NetworkObject>().objectName, foo, 0, boo);
    }

}

