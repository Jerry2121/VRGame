using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            VRGame.Mail.Email mail = new VRGame.Mail.Email("smtp-mail.outlook.com", "19.Aaron.Wiens@ksd.org", "Frozen312");
            Debug.Log("Foo");
            mail.Send();
        }
    }
}
