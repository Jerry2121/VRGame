using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Controller : MonoBehaviour
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


    private void Update()
    {
        if(!PuzzleCompleted && !PuzzleStarted && PuzzleWheel.GetComponent<Puzzle1WheelCollider>().Spins > 5 && !PuzzlePoweredUp)
        {
            PuzzlePoweredUp = true;
            PuzzleLightingBolt.SetActive(true);
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
