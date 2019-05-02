using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronBehavior : MonoBehaviour
{
    public bool mixtureFinished;

    public int correctMixturesNeeded;
    public int correctMixturesCompleted;
    public int mixtureNeededID;
    public float addToMixCooldown;
    float mixCooldown;

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
        correctMixturesCompleted = 0;
        mixCooldown = 0;
        markingLerping = false;
        markingLerp = 0;
        mixtureNeededID = Random.Range(0, 6);
        cauldronMarking.GetComponent<Renderer>().material.color = cauldronMarkingsArray[mixtureNeededID];
    }

    void Update()
    {
        mixCooldown -= Time.deltaTime;
        

        if (correctMixturesCompleted >= correctMixturesNeeded)
        {
            mixtureFinished = true;
            cauldronMarking.GetComponent<Renderer>().material.color = new Color32(0, 0, 0, 0);
        }
        Debug.Log("Correct Mixtures Completed: " + correctMixturesCompleted);
        Debug.Log("Correct mixtures needed: " + correctMixturesNeeded);
        fillLerp = (float)correctMixturesCompleted / correctMixturesNeeded;
        Debug.Log(fillLerp);
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
        if (mixCooldown > 0)
        {
            return;
        }

        if (mixtureFinished)
        {
            return;
        }

        if (liquidID == mixtureNeededID)
        {
            Debug.Log("DING DING");
            correctMixturesCompleted++;
            mixCooldown = addToMixCooldown;
        }

        else if (liquidID != mixtureNeededID)
        {
            Debug.Log("donk donk");
            correctMixturesCompleted = 0;
            mixCooldown = addToMixCooldown;
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