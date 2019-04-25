using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFadeText : MonoBehaviour
{
    public float TimerFadeInOut;
    public bool complete;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Fade_In", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!complete)
        {
            TimerFadeInOut += Time.deltaTime;
        }
        if (TimerFadeInOut >= 3)
        {
            complete = true;
            this.gameObject.GetComponent<Animator>().SetBool("Fade_In", false);
            this.gameObject.GetComponent<Animator>().SetBool("Fade_Out", true);
        }
    }
}
