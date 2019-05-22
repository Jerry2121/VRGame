using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPuzzleBehavior : MonoBehaviour
{
    public GameObject[] crystalReceptors;
    public bool puzzleCompleted;
    
    void Update()
    {
        if (puzzleCompleted)
        {
            return;
        }

        for (int i = 0; i < crystalReceptors.Length; i++)
        {
            if (crystalReceptors[i].GetComponent<CrystalReceptorBehavior>().crystalRecieved = null)
            {
                return;
            }
        }

        puzzleCompleted = true;
    }
}
