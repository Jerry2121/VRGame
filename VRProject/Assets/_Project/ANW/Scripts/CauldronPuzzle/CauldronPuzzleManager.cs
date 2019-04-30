using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronPuzzleManager : MonoBehaviour
{
    public GameObject[] Bottles;
    public GameObject[] Cauldrons;
    public Transform[] BottlePositions;

    public bool[] mixtureCheck;

    public bool CheckPuzzle()
    {
        for (int i = 0; i < Cauldrons.Length; i++) 
        {
            if (!Cauldrons[i].GetComponent<CauldronBehavior>().CorrectMixture)
            {
                return false;
            }
        }
        return true;
    }

    void ResetPuzzle()
    {
        for (int i = 0; i < Bottles.Length; i++)
        {
            Bottles[i].transform.position = BottlePositions[i].position;
            Bottles[i].transform.rotation = BottlePositions[i].rotation;
            //Bottles[i].GetComponent<MeshRenderer>()
        }
    }
}
