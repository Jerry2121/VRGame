using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFadeText : MonoBehaviour
{
    public float TimerFadeInOut;
    public bool RoomNameComplete;
    public bool RoomObjectiveComplete;
    public bool ObjectiveComplete;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Fade_In", true);
        this.gameObject.GetComponent<Animator>().SetBool("Fade_Out", false);
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
            this.gameObject.GetComponent<Animator>().SetBool("Fade_In", false);
            this.gameObject.GetComponent<Animator>().SetBool("Fade_Out", true);
            RoomNameComplete = true;
            TimerFadeInOut = 0;
        }
        if (!RoomObjectiveComplete && ObjectiveComplete)
        {
            TimerFadeInOut += Time.deltaTime;
        }
        if (ObjectiveComplete && TimerFadeInOut < 3 && !RoomObjectiveComplete)
        {
            this.gameObject.GetComponent<Animator>().SetBool("ObjectiveFade_In", true);
            this.gameObject.GetComponent<Animator>().SetBool("ObjectiveFade_Out", false);
        }
        if (TimerFadeInOut >= 3 && ObjectiveComplete && !RoomObjectiveComplete)
        {
            this.gameObject.GetComponent<Animator>().SetBool("ObjectiveFade_In", false);
            this.gameObject.GetComponent<Animator>().SetBool("ObjectiveFade_Out", true);
            RoomObjectiveComplete = true;
            TimerFadeInOut = 0;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ObjectiveComplete = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            RoomObjectiveComplete = true;
        }
    }
}
