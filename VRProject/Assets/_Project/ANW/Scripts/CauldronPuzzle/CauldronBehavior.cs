using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronBehavior : MonoBehaviour
{
    public bool mixtureFinished;

    public int correctMixturesNeeded;
    public int correctMixturesCompleted;
    public int mixtureNeededID;

    public GameObject cauldronMarking;

    public Color32[] cauldronMarkingsArray;
    // public Color32[] cauldronFillArray;

    float colorLerp;
    bool lerping;


    private void Start()
    {
        lerping = false;
        colorLerp = 0;
        mixtureNeededID = Random.Range(0, 6);
        cauldronMarking.GetComponent<Renderer>().material.color = cauldronMarkingsArray[Random.Range(0, 6)];
    }

    void Update()
    {
        if (correctMixturesCompleted >= correctMixturesNeeded)
        {
            mixtureFinished = true;
            cauldronMarking.GetComponent<Renderer>().material.color = new Color32(0, 0, 0, 0);
        }

        if (lerping)
        {
            colorLerp += Time.deltaTime / 2.0f;
            cauldronMarking.GetComponent<Renderer>().material.color = Color.Lerp(cauldronMarking.GetComponent<Renderer>().material.color, cauldronMarkingsArray[mixtureNeededID], colorLerp);
            if (colorLerp >= 1)
            {
                lerping = false;
                colorLerp = 0;
            }
        }
    }

    public void CheckMixture(int liquidID)
    {
        if (mixtureFinished)
        {
            return;
        }

        if (liquidID == mixtureNeededID)
        {
            correctMixturesCompleted++;
        }
        else
        {
            correctMixturesCompleted = 0;
        }
        UpdateMixture(liquidID);
    }

    public void UpdateMixture(int liquidID)
    {
        mixtureNeededID = Random.Range(0, 6);
        
        if (mixtureNeededID == liquidID)
        {
            UpdateMixture(liquidID);
            return;
        }

        lerping = true;
    }
}


