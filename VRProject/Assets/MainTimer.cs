using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTimer : MonoBehaviour
{
    //PlayerPrefs
    //30mTimer
    //TimeElapsed
    private float TimerPlaceHolder = 1800;
    private float ThirtyTimerPlaceHolder;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("30mTimer", 0);
        PlayerPrefs.SetFloat("TimeElapsedTimer", 0);
    }

    // Update is called once per frame
    void Update()
    {
        ThirtyTimerPlaceHolder = PlayerPrefs.GetFloat("30mTimer");
        if (PlayerPrefs.GetInt("ThirtyMinuteTimer") == 1)
        {
            TimerPlaceHolder -= Time.deltaTime;
            double b;
            b = System.Math.Round(TimerPlaceHolder, 0);
            PlayerPrefs.SetFloat("30mTimer", (int)b);
        }
        if (PlayerPrefs.GetInt("TimeElapsed") == 1)
        {
            ThirtyTimerPlaceHolder += Time.deltaTime;
            double b;
            b = System.Math.Round(ThirtyTimerPlaceHolder, 0);
            PlayerPrefs.SetFloat("TimeElapsedTimer", (int)b);
        }
    }
}
