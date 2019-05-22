using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;

public class CrystalPuzzleBehavior : MonoBehaviour
{
    public GameObject[] crystalReceptors;
    public bool puzzleCompleted;
    public GameObject LevelInformationCanvas;
    
    void Update()
    {
        if (LevelInformationCanvas == null)
        {
            if (NetworkingManager.GetLocalPlayer() != null)
            {
                LevelInformationCanvas = NetworkingManager.GetLocalPlayer().GetComponentInChildren<RoomFadeText>().gameObject;
            }
            else
            {
                return;
            }
        }
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
        LevelInformationCanvas.GetComponent<RoomFadeText>().RoomObjectiveComplete = true;
        puzzleCompleted = true;
    }
}
