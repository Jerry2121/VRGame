using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPauseMenu : MonoBehaviour
{
    public GameObject PauseCanvas;
    public GameObject PauseObjects;
    public GameObject Player;
    public GameObject ExitConfirm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            Pause();
        }
    }
    public void Pause()
    {
        //Turn on/off the PauseCanvas and reset its position to be in front of the player based on its active state.
        PauseCanvas.transform.position = Player.transform.position + new Vector3(0, 0, 1);
        PauseCanvas.transform.rotation = Player.transform.rotation;
        PauseCanvas.SetActive(!PauseCanvas.activeSelf);
    }
    public void Resume()
    {
        //Turn on/off the PauseCanvas based on its active state and turn off the ExitConfirm regardless of its state.
        PauseCanvas.SetActive(!PauseCanvas.activeSelf);
        ExitConfirm.SetActive(false);
    }
    public void Settings()
    {
        //do nothing until Settings gets coded and implamented.
        Debug.Log("Settings Button Clicked!!");
    }
    public void Disconnect()
    {
        //Turn off the PauseMenu objects.
        PauseObjects.SetActive(false);
        //Turn on/off the ExitConfirm Canvas based on its active state
        ExitConfirm.SetActive(!ExitConfirm.activeSelf);
    }
    public void ExitConfirmYes()
    {
        //load MainMenu if Yes Button is clicked
        SceneManager.LoadScene(0);
    }
    public void ExitConfirmNo()
    {
        //Turn on the PauseMenu Objects.
        PauseObjects.SetActive(true);
        //Turn on/off the ExitConfirm Canvas based on its active state
        ExitConfirm.SetActive(!ExitConfirm.activeSelf);
    }
}
