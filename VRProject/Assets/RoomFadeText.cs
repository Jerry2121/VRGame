using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFadeText : MonoBehaviour
{
    public float TimerFadeInOut;
    public bool RoomNameComplete;
    public bool RoomObjectiveComplete;
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
        if (Input.GetKeyDown(KeyCode.L))
        {
            RoomObjectiveComplete = true;
        }
    }
}
