using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPuzzleBehavior : MonoBehaviour
{
    public GameObject[] crystalReceptors;
    public GameObject crystalRecieved;
    public bool puzzleCompleted;

    private void Update()
    {
        if (puzzleCompleted)
        {
            return;
        }

        for (int i = 0; i < crystalReceptors.Length; i++)
        {
            if (crystalReceptors[i].GetComponent<CrystalPuzzleBehavior>().crystalRecieved = null)
            {
                return;
            }
        }

        puzzleCompleted = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == crystalRecieved)
        {
            crystalRecieved = null;
        }
    }
}
