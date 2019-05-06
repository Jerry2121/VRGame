using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;

public class Puzzle1Controller : NetworkedPuzzleController
{
    public bool PuzzleStarted;
    public bool PuzzleCompleted;
    public bool PuzzlePoweredUp;
    public bool ProgressMade;
    public bool ProgressMade1;
    public bool ProgressMade2;
    public GameObject PuzzleWheel;
    public Light Puzzle1Light;
    private bool ran1;
    public AudioSource PuzzlePowerUp;
    public AudioSource PuzzleWrong;
    public AudioSource PuzzleCompletedSound;
    public GameObject PuzzleLightingBolt;
    public GameObject PuzzleLightingBolt2;

    public override int ID { get => m_ID; protected set => m_ID = value; }

    private int m_ID;

    private void Update()
    {
        if(!PuzzleCompleted && !PuzzleStarted && PuzzleWheel.GetComponent<Puzzle1WheelCollider>().Spins > 5 && !PuzzlePoweredUp)
        {
            PuzzlePoweredUp = true;
            PuzzleLightingBolt.SetActive(true);
            SendNetworkMessage(NetworkTranslater.CreatePuzzleStartedMessage(NetworkingManager.ClientID(), m_ID));
        }
        if (PuzzlePoweredUp && !ran1)
        {
            Puzzle1Light.color = new Color32(0, 77, 77, 255);
            PuzzlePowerUp.Play();
            ran1 = true;
        }
        if (ProgressMade1 && ProgressMade2)
        {
            ProgressMade = true;
        }
    }

    public void PuzzleStart()
    {
        Puzzle1Light.color = new Color32(0, 57, 255, 255);
        PuzzleLightingBolt.SetActive(false);
        PuzzleLightingBolt2.SetActive(true);
        PuzzleStarted = true;
    }

    public void PuzzleFailed()
    {
        Puzzle1Light.color = new Color32(255, 0, 0, 255);
        PuzzleWrong.Play();
        PuzzleLightingBolt.SetActive(true);
        PuzzleLightingBolt2.SetActive(false);
        PuzzleStarted = false;
        ProgressMade1 = false;
        ProgressMade2 = false;
        ProgressMade = false;
    }

    public void PuzzleComplete()
    {
        Puzzle1Light.color = new Color32(0, 65, 9, 255);
        PuzzleCompletedSound.Play();
        PuzzleStarted = false;
        PuzzleCompleted = true;
    }

    public override void RecieveNetworkMessage(string recievedMessage)
    {
        switch (NetworkTranslater.GetMessageContentType(recievedMessage))
        {
            case NetworkMessageContent.PuzzleStarted:
                Debug.Log("Puzzle1Controller -- RecieveNetworkMessage: Recieved PuzzleStarted message");
                PuzzlePoweredUp = true;
                PuzzleLightingBolt.SetActive(true);
                break;

            case NetworkMessageContent.PuzzleProgress:
                Debug.Log("Puzzle1Controller -- RecieveNetworkMessage: Recieved PuzzleProgress message");
                break;

            case NetworkMessageContent.PuzzleComplete:
                Debug.Log("Puzzle1Controller -- RecieveNetworkMessage: Recieved PuzzleComplete message");
                break;
        }

    }

    public override void SendNetworkMessage(string messageToSend)
    {
        NetworkingManager.s_Instance.SendNetworkMessage(messageToSend);
    }

    public override void SetID(int newID)
    {
        if (newID > -1)
        {
            if (Debug.isDebugBuild)
                Debug.Log(string.Format("Puzzle1Controller -- SetID: ID set to {0}", newID));
            ID = newID;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "P1Start" && !PuzzleStarted && !PuzzleCompleted && PuzzlePoweredUp)
    //    {
    //        Puzzle1Light.color = new Color32(0, 57, 255, 255);
    //        PuzzleStarted = true;
    //    }
    //    if(other.gameObject.tag == "P1Bar" && PuzzleStarted && PuzzlePoweredUp)
    //    {
    //        Debug.Log("Puzzle Wrong" + other.gameObject.name);
    //        Puzzle1Light.color = new Color32(255, 0, 0, 255);
    //        PuzzleWrong.Play();
    //        PuzzleStarted = false;

    //    }
    //    if (other.gameObject.tag == "P1End" && PuzzleStarted && PuzzlePoweredUp && ProgressMade)
    //    {
    //        Puzzle1Light.color = new Color32(0, 65, 9, 255);
    //        PuzzleCompletedSound.Play();
    //        PuzzleStarted = false;
    //        PuzzleCompleted = true;
    //    }
    //    if (other.gameObject.tag == "P1Progress1")
    //    {
    //        ProgressMade1 = true;
    //    }
    //    if (other.gameObject.tag == "P1Progress2")
    //    {
    //        ProgressMade2 = true;
    //    }
    //}
}
