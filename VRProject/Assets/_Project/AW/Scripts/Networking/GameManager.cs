using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;


public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance;

    [SerializeField]
    NetworkedPuzzleController[] m_PuzzleControllers;

    // Start is called before the first frame update
    void Start()
    {
        if (s_Instance != null)
        {
            Debug.LogWarning("GameManager -- Start: Instance was not equal to null! Destroying this component!");
            Destroy(this);
            return;
        }
        s_Instance = this;

        for (int i = 0; i < m_PuzzleControllers.Length; i++)
        {
            m_PuzzleControllers[i].SetID(i);
        }

    }

    public void PassPuzzleNetworkMessage(string recievedMessage, int puzzleID)
    {
        m_PuzzleControllers[puzzleID].RecieveNetworkMessage(recievedMessage);
    }
}
