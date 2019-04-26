using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandElectricPuzzle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter -- Wand");

        Puzzle1Controller controller = other.GetComponentInParent<TrackElectricPuzzle>()?.puzzleController;

        if (controller == null) return;

        Debug.Log("OnTriggerEnter -- Control not NULL");

        if (other.gameObject.tag == "P1Start" && !controller.PuzzleStarted && !controller.PuzzleCompleted && controller.PuzzlePoweredUp)
        {
            controller.PuzzleStart();
        }
        else if (other.gameObject.tag == "P1Bar" && controller.PuzzleStarted && controller.PuzzlePoweredUp)
        {
            controller.PuzzleFailed();
        }
        else if (other.gameObject.tag == "P1End" && controller.PuzzleStarted && controller.PuzzlePoweredUp && controller.ProgressMade)
        {
            controller.PuzzleComplete();
        }
        else if (other.gameObject.tag == "P1Progress1")
        {
            controller.ProgressMade1 = true;
        }
        else if (other.gameObject.tag == "P1Progress2")
        {
            controller.ProgressMade2 = true;
        }
    }

}
