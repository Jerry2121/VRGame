using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUIController : MonoBehaviour
{
    public GameObject LevelInformationCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Room2Trigger")
        {
            LevelInformationCanvas.GetComponent<RoomFadeText>().Room2Entered = true;
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.name == "Room3Trigger")
        {
            LevelInformationCanvas.GetComponent<RoomFadeText>().Room3Entered = true;
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.name == "Room4Trigger")
        {
            LevelInformationCanvas.GetComponent<RoomFadeText>().Room4Entered = true;
            other.gameObject.SetActive(false);
        }
    }
}
