using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        PutOnHeadSetCanvas.SetActive(true);
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
    public void ExitGame()
    {
        ExitConfirm.SetActive(!ExitConfirm.activeSelf);
    }
    public void ExitYes()
    {
        Application.Quit();
    }
}
