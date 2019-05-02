using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;

public class GameManager : MonoBehaviour
{
    static GameManager s_Instance;

    List<INetworkMessageReciever> m_PuzzleControllers;

    // Start is called before the first frame update
    void Start()
    {
        if(s_Instance != null)
        {
            Debug.LogWarning("GameManager -- Start: Instance was not equal to null! Destroying this component!");
            Destroy(this);
            return;
        }
        s_Instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
