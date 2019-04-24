using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Controller : MonoBehaviour
{
    public bool PuzzleStarted;
    public bool PuzzleCompleted;
    public bool PuzzlePoweredUp;
    public GameObject PuzzleWheel;
    public Light Puzzle1Light;
    private bool ran1;
    public AudioSource PuzzlePowerUp;
    public AudioSource PuzzleWrong;
    public AudioSource PuzzleCompletedSound;


    private void Update()
    {
        if(!PuzzleCompleted && !PuzzleStarted && PuzzleWheel.GetComponent<Puzzle1WheelCollider>().Spins > 5 && !PuzzlePoweredUp)
        {
            PuzzlePoweredUp = true;
        }
        if (PuzzlePoweredUp && !ran1)
        {
            Puzzle1Light.color = new Color32(0, 255, 0, 255);
            PuzzlePowerUp.Play();
            ran1 = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "P1Start" && !PuzzleStarted && !PuzzleCompleted && PuzzlePoweredUp)
        {
            Puzzle1Light.color = new Color32(0, 57, 255, 255);
            PuzzleStarted = true;
        }
        if(other.gameObject.tag == "P1Bar" && PuzzleStarted && PuzzlePoweredUp)
        {
            Puzzle1Light.color = new Color32(255, 0, 0, 255);
            PuzzleWrong.Play();
            PuzzleStarted = false;

        }
        if (other.gameObject.tag == "P1End" && PuzzleStarted && PuzzlePoweredUp)
        {
            Puzzle1Light.color = new Color32(255, 81, 0, 255);
            PuzzleCompletedSound.Play();
            PuzzleStarted = false;
            PuzzleCompleted = true;
        }
    }
}
