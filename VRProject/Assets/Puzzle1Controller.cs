using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Controller : MonoBehaviour
{
    public bool PuzzleStarted;
    public bool PuzzleCompleted;
    public bool PuzzlePoweredUp;
    public Light Puzzle1Light;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "P1Start" && !PuzzleStarted && !PuzzleCompleted && PuzzlePoweredUp)
        {
            Puzzle1Light.color = new Color32(0, 255, 0, 255);
            PuzzleStarted = true;
        }
        if(other.gameObject.tag == "P1Bar" && PuzzleStarted && PuzzlePoweredUp)
        {
            Puzzle1Light.color = new Color32(255, 0, 0, 255);
            PuzzleStarted = false;

        }
        if (other.gameObject.tag == "P1End" && PuzzleStarted && PuzzlePoweredUp)
        {
            Puzzle1Light.color = new Color32(0, 0, 255, 255);
            PuzzleStarted = false;
            PuzzleCompleted = true;
        }
    }
}
