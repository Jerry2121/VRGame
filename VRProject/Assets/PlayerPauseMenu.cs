using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPauseMenu : MonoBehaviour
{
    public GameObject PauseCanvas;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            Debug.Log("foo");
            PauseCanvas.transform.position = Player.transform.position + new Vector3(0,0,1);
            PauseCanvas.transform.rotation = Player.transform.rotation;
            PauseCanvas.SetActive(!PauseCanvas.activeSelf);
        }
    }
}
