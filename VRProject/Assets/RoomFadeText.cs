using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFadeText : MonoBehaviour
{
    public float TimerFadeInOut;
    public bool RoomNameComplete;
    public bool RoomObjectiveComplete;
    public bool Room2Entered;
    public bool Room3Entered;
    public bool Room4Entered;
    private bool Room2Ran;
    private bool Room3Ran;
    private bool Room4Ran;
    public Animator UIInfo;
    // Start is called before the first frame update
    void Start()
    {
        UIInfo.SetBool("Fade_In", true);
        UIInfo.SetBool("Fade_Out", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            RoomObjectiveComplete = true;
        }
        //Room1 Controller
        if (!RoomNameComplete)
        {
            TimerFadeInOut += Time.deltaTime;
        }
        if (TimerFadeInOut >= 3 && !RoomNameComplete)
        {
            UIInfo.SetBool("Fade_In", false);
            UIInfo.SetBool("Fade_Out", true);
            RoomNameComplete = true;
            TimerFadeInOut = 0;
        }

        //Room 2 Controller
        if (Room2Entered && !Room2Ran)
        {
            TimerFadeInOut += Time.deltaTime;
        }
        if (TimerFadeInOut >= 3 && Room2Entered && !Room2Ran)
        {
            UIInfo.SetBool("Room2Fade_In", false);
            UIInfo.SetBool("Room2Fade_Out", true);
            Room2Ran = true;
            TimerFadeInOut = 0;
        }
        else if (Room2Entered && TimerFadeInOut < 3 && !Room2Ran)
        {
            UIInfo.SetBool("Room2Fade_In", true);
            UIInfo.SetBool("Room2Fade_Out", false);
        }


        //Room 3 Controller
        if (Room3Entered && !Room3Ran)
        {
            TimerFadeInOut += Time.deltaTime;
        }
        if (TimerFadeInOut >= 3 && Room3Entered && !Room3Ran)
        {
            UIInfo.SetBool("Room3Fade_In", false);
            UIInfo.SetBool("Room3Fade_Out", true);
            Room3Ran = true;
            TimerFadeInOut = 0;
        }
        else if (Room3Entered && TimerFadeInOut < 3 && !Room3Ran)
        {
            UIInfo.SetBool("Room3Fade_In", true);
            UIInfo.SetBool("Room3Fade_Out", false);
        }


        //Room 4 Controller
        if (Room4Entered && !Room4Ran)
        {
            TimerFadeInOut += Time.deltaTime;
        }
        if (TimerFadeInOut >= 3 && Room4Entered && !Room4Ran)
        {
            UIInfo.SetBool("Room4Fade_In", false);
            UIInfo.SetBool("Room4Fade_Out", true);
            Room4Ran = true;
            TimerFadeInOut = 0;
        }
        else if (Room4Entered && TimerFadeInOut < 3 && !Room4Ran)
        {
            UIInfo.SetBool("Room4Fade_In", true);
            UIInfo.SetBool("Room4Fade_Out", false);
        }


        //Objective Complete Controller
        if (RoomObjectiveComplete)
        {
            TimerFadeInOut += Time.deltaTime;
        }
        if (RoomObjectiveComplete && TimerFadeInOut < 3)
        {
            UIInfo.SetBool("ObjectiveFade_In", true);
            UIInfo.SetBool("ObjectiveFade_Out", false);
        }
        if (TimerFadeInOut >= 3 && RoomObjectiveComplete)
        {
            UIInfo.SetBool("ObjectiveFade_In", false);
            UIInfo.SetBool("ObjectiveFade_Out", true);
            TimerFadeInOut = 0;
            RoomObjectiveComplete = false;
        }
    }
}
