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
        float x;
        float z;

        switch (NetworkTranslater.GetMessageContentType(msg))
        {
            case NetworkMessageContent.Move:
                if (NetworkTranslater.TranslateMoveMessage(msg, out int ID, out x, out z))
                    Debug.LogFormat("x = {0}, z = {1}", x, z);
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
