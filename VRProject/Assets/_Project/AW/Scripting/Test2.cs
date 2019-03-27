using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using VRGame.Networking;

public class Test2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string> listone = new List<string>();
        listone.Add("Foo");
        listone.Add("Roo");
        listone.Add("Too");
        listone.Add("Yoo");

        List<string> listtwo = new List<string>(listone);

        listone.Clear();

        Debug.Log("List One:");
        foreach(var v in listone)
        {
            Debug.Log(v);
        }
        Debug.Log("List Two:");
        foreach (var v in listtwo)
        {
            Debug.Log(v);
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
    
}

