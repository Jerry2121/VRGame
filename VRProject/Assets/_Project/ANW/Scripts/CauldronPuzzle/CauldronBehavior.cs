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
    public GameObject cauldronFill;

    public Color32[] cauldronMarkingsArray;
    public Color32 cauldronFillStart;
    public Color32 cauldronFillEnd;

    float markingLerp;
    bool markingLerping;

    float fillLerp;


    private void Start()
    {
        markingLerping = false;
        markingLerp = 0;
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

        fillLerp = correctMixturesCompleted / correctMixturesNeeded;
        cauldronFill.GetComponent<Renderer>().material.color = Color.Lerp(cauldronFillStart, cauldronFillEnd, fillLerp);

        if (markingLerping)
        {
            markingLerp += Time.deltaTime / 2.0f;
            cauldronMarking.GetComponent<Renderer>().material.color = Color.Lerp(cauldronMarking.GetComponent<Renderer>().material.color, cauldronMarkingsArray[mixtureNeededID], markingLerp);
            if (markingLerp >= 1)
            {
                markingLerping = false;
                markingLerp = 0;
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

        markingLerping = true;
    }
}


