using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;

public class MainTimer : MonoBehaviour
{
    //PlayerPrefs
    //30mTimer
    //TimeElapsed
    public GameObject EndGameCanvas;
    public GameObject WinObject;
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
        Debug.Log("Player Pref ThirtyMinuteTimer value = " + PlayerPrefs.GetInt("ThirtyMinuteTimer"));
        Debug.Log("Player Pref TimeElapsed value = " + PlayerPrefs.GetInt("TimeElapsed"));
        if (EndGameCanvas == null)
        {
            if (NetworkingManager.GetLocalPlayer() != null)
            {
                EndGameCanvasScript canvasscript = NetworkingManager.GetLocalPlayer().GetComponentInChildren<EndGameCanvasScript>();
                WinObject = canvasscript.WinObject;
            }
            else
            {
                return;
            }
        }
        ThirtyTimerPlaceHolder = PlayerPrefs.GetFloat("30mTimer");
        if (PlayerPrefs.GetInt("ThirtyMinuteTimer") == 1 && WinObject == !isActiveAndEnabled)
        {
            TimerPlaceHolder -= Time.deltaTime;
            double b;
            b = System.Math.Round(TimerPlaceHolder, 0);
            PlayerPrefs.SetFloat("30mTimer", (int)b);
        }
        if (PlayerPrefs.GetInt("TimeElapsed") == 1 && WinObject == !isActiveAndEnabled)
        {
            ThirtyTimerPlaceHolder += Time.deltaTime;
            double b;
            b = System.Math.Round(ThirtyTimerPlaceHolder, 0);
            PlayerPrefs.SetFloat("TimeElapsedTimer", (int)b);
        }
    }
}
