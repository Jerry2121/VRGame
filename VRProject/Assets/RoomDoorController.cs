using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;

public class RoomDoorController : MonoBehaviour
{
    public GameObject LevelInformationCanvas;
    private bool ran;
    public Animator Room1_2Doorway;
    public Animator Room1_2Doorway2;
    public Animator Room2_3Doorway;
    public Animator Room2_3Doorway2;
    public Animator Room3_4Doorway;
    public Animator Room3_4Doorway2;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelInformationCanvas == null)
        {
            if(NetworkingManager.GetLocalPlayer() != null)
            {
                LevelInformationCanvas = NetworkingManager.GetLocalPlayer().GetComponentInChildren<RoomFadeText>().gameObject;
            }
            else
            {
                return;
            }
        }
        if (LevelInformationCanvas.GetComponent<RoomFadeText>().RoomObjectiveComplete && !ran)
        {
            Debug.Log("Room12DoorOpened");
            Room1_2Doorway.SetBool("Open", true);
            Room1_2Doorway2.SetBool("Open", true);
            ran = true;
        }
        if (LevelInformationCanvas.GetComponent<RoomFadeText>().RoomObjectiveComplete && LevelInformationCanvas.GetComponent<RoomFadeText>().Room2Entered)
        {
            Debug.Log("Room23DoorOpened");
            Room2_3Doorway.SetBool("Open", true);
            Room2_3Doorway2.SetBool("Open", true);
        }
        if (LevelInformationCanvas.GetComponent<RoomFadeText>().RoomObjectiveComplete && LevelInformationCanvas.GetComponent<RoomFadeText>().Room3Entered)
        {
            Debug.Log("Room34DoorOpened");
            Room3_4Doorway.SetBool("Open", true);
            Room3_4Doorway2.SetBool("Open", true);
        }
    }
}
