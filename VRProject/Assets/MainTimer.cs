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
        if (EndGameCanvas == null)
        {
            if (NetworkingManager.GetLocalPlayer() != null)
            {
                EndGameCanvas = NetworkingManager.GetLocalPlayer().GetComponentInChildren<EndGameCanvasScript>().gameObject;
                EndGameCanvasScript canvasscript = NetworkingManager.GetLocalPlayer().GetComponentInChildren<EndGameCanvasScript>();
                WinObject = canvasscript.WinObject;
            }
            else
            {
                return;
            }
        }
        ThirtyTimerPlaceHolder = PlayerPrefs.GetFloat("30mTimer");
        if (PlayerPrefs.GetInt("ThirtyMinuteTimer") == 1 && GameObject.Find("1LeverHandle(Clone)").GetComponent<LeverScript>().GameOver == false || PlayerPrefs.GetInt("ThirtyMinuteTimer") == 1 && GameObject.Find("2LeverHandle(Clone)").GetComponent<LeverScript>().GameOver == false)
        {
            TimerPlaceHolder -= Time.deltaTime;
            float b;
            b = TimerPlaceHolder;
            PlayerPrefs.SetFloat("30mTimer", b);
        }
        ThirtyTimerPlaceHolder = PlayerPrefs.GetFloat("TimeElapsedTimer");
        if (PlayerPrefs.GetInt("TimeElapsed") == 1 && GameObject.Find("1LeverHandle(Clone)").GetComponent<LeverScript>().GameOver == false || PlayerPrefs.GetInt("TimeElapsed") == 1 && GameObject.Find("2LeverHandle(Clone)").GetComponent<LeverScript>().GameOver == false)
        {
            ThirtyTimerPlaceHolder += Time.deltaTime;
            float b;
            b = ThirtyTimerPlaceHolder;
            PlayerPrefs.SetFloat("TimeElapsedTimer", b);
        }
    }
}
