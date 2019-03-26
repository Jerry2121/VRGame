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
        string msg = "1|Move|9.4|5.4";
        string msgtwo = "1|Pos|6.77|3.44";
        float x;
        float z;

        string sentmsg = NetworkTranslater.CombineMessages(new string[] { msg, msgtwo });

        Debug.Log(sentmsg);

        string[] recieved = NetworkTranslater.SplitMessages(sentmsg);

        foreach(var msgr in recieved)
        {
            Debug.Log(msgr);
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
    
}

