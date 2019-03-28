using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using VRGame.Networking;

public class Test2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string foo = string.Empty;

        byte[] msgbytes = Encoding.Unicode.GetBytes(foo);

        if (Encoding.Unicode.GetString(msgbytes) == string.Empty)
            Debug.LogError("Empty");
        else if (string.IsNullOrWhiteSpace(Encoding.Unicode.GetString(msgbytes)))
            Debug.LogError("Null or Whitespace");
        else
            Debug.LogError("??? " + Encoding.Unicode.GetString(msgbytes));

    }

    // Update is called once per frame
    void Update()
    {
    }
    
}

