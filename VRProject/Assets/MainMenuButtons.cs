using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using VRGame.Networking;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject MainCanvas;
    public GameObject PlayCanvas;
    public GameObject PlayMain;
    public GameObject PlayHost;
    public GameObject PlayJoin;
    public GameObject SettingsCanvas;
    public GameObject ExitConfirm;
    public GameObject PutOnHeadSetCanvas;
    [Header("HostGameOptions")]
    public TMP_InputField WorldName;
    public TMP_InputField PlayerUsername;
    public Toggle AllowHintsToggle;
    public Toggle thirtyminutetimer;
    public Toggle timeelapsedtimer;
    public GameObject Warning;
    // Start is called before the first frame update
    void Start()
    {
        PutOnHeadSetCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        MainCanvas.SetActive(!MainCanvas.activeSelf);
        PlayCanvas.SetActive(!PlayCanvas.activeSelf);
        PlayMain.SetActive(true);
        PlayHost.SetActive(false);
        PlayJoin.SetActive(false);
    }
    public void HostGame()
    {
        PlayMain.SetActive(false);
        PlayHost.SetActive(true);
        PlayJoin.SetActive(false);
    }
    public void CreateGameTEST()
    {
        if (string.IsNullOrWhiteSpace(WorldName.text) == false && string.IsNullOrWhiteSpace(PlayerUsername.text) == false)
        {
            PlayerPrefs.SetString("WorldName", WorldName.text);
            PlayerPrefs.SetString("PlayerUsername", PlayerUsername.text);
            if (AllowHintsToggle.isOn)
            {
                PlayerPrefs.SetInt("AllowHints", 1);
            }
            else
            {
                PlayerPrefs.SetInt("AllowHints", 0);
            }

            if (thirtyminutetimer.isOn)
            {
                PlayerPrefs.SetInt("ThirtyMinuteTimer", 1);
            }
            else
            {
                PlayerPrefs.SetInt("ThirtyMinuteTimer", 0);
            }

            if (timeelapsedtimer.isOn)
            {
                PlayerPrefs.SetInt("TimeElapsed", 1);
            }
            else
            {
                PlayerPrefs.SetInt("TimeElapsed", 0);
            }
            Warning.SetActive(false);
            PutOnHeadSetCanvas.SetActive(true);

            if (NetworkingManager.s_Instance != null)
            {
                NetworkingManager.s_Instance.StartHost();
            }
            else
                SceneManager.LoadScene(1);
        }
        else
        {
            Warning.SetActive(true);
        }
    }

    public void JoinGameNetwork()
    {
        if (NetworkingManager.s_Instance != null)
            NetworkingManager.s_Instance.StartClient();
        else
            SceneManager.LoadScene(1);
    }

    public void JoinGame()
    {
        PlayMain.SetActive(false);
        PlayHost.SetActive(false);
        PlayJoin.SetActive(true);
    }
    public void GoBackMain()
    {
        MainCanvas.SetActive(!MainCanvas.activeSelf);
        PlayCanvas.SetActive(!PlayCanvas.activeSelf);
        PlayMain.SetActive(false);
        PlayHost.SetActive(false);
        PlayJoin.SetActive(false);
    }
    public void GoBackHost()
    {
        PlayMain.SetActive(true);
        PlayHost.SetActive(false);
        PlayJoin.SetActive(false);
    }
    public void GoBackJoin()
    {
        PlayMain.SetActive(true);
        PlayHost.SetActive(false);
        PlayJoin.SetActive(false);
    }
    public void Settings()
    {

    }

    public void SetIP(string ip)
    {
        NetworkingManager.s_Instance.SetIPAddress(ip);
    }

    public void ExitGame()
    {
        ExitConfirm.SetActive(!ExitConfirm.activeSelf);
    }
    public void ExitYes()
    {
        Application.Quit();
    }
}
