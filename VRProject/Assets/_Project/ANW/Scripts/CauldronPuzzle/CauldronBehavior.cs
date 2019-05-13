using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;

public class CauldronBehavior : NetworkObjectComponent
{
    // Whether or not the mixture is finished
    public bool mixtureFinished;

    // How many mixtures are needed? (set in the inspector)
    [HideInInspector]public int correctMixturesNeeded = 5;
    //How many mixtures have been completed?
    [HideInInspector]public int correctMixturesCompleted;
    // What bottle needs to be poured into the cauldron
    [HideInInspector]public int mixtureNeededID;
    public float addToMixCooldown;
    float mixCooldown;

    public GameObject cauldronMarking;
    public GameObject cauldronFill;

    public GameObject paper;
    [HideInInspector] public Material[] paperMarkings;
    public Material[] paperMarkingsArray;
    public Material paperNullMarking;

    public Color32[] cauldronMarkingsArray;    
    public Color32 cauldronFillStart;
    public Color32 cauldronFillEnd;

    float markingLerp;
    bool markingLerping;

    float fillLerp;

    public GameObject powerBreaker;
    public bool buttonMix;

    public override NetworkObject m_NetworkObject { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override int ID { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    [HideInNormalInspector]
    [SerializeField]
    int m_ID;

    private void Start()
    {
        correctMixturesCompleted = 0;
        mixCooldown = 0;
        markingLerping = false;
        markingLerp = 0;
        mixtureNeededID = Random.Range(0, 6);
        cauldronMarking.GetComponent<Renderer>().material.color = cauldronMarkingsArray[mixtureNeededID];
        paperMarkings = paper.GetComponent<Renderer>().materials;
        powerBreaker = GameObject.Find("PowerBreaker");
    }

    void Update()
    {
        mixCooldown -= Time.deltaTime;
        

        if (correctMixturesCompleted >= correctMixturesNeeded)
        {
            mixtureFinished = true;
            cauldronMarking.GetComponent<Renderer>().material.color = new Color32(0, 0, 0, 0);
            if (buttonMix)
            {
                for (int i = 0; i < powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored.Length; i++)
                {
                    int tmp = powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored[i];
                    int j = Random.Range(i, powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored.Length);
                    powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored[i] = powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored[j];
                    powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored[j] = tmp;
                }

                buttonMix = false;
            }
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
            powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored[correctMixturesCompleted] = liquidID;
            correctMixturesCompleted++;
            paperMarkings[correctMixturesCompleted] = paperMarkingsArray[liquidID];
            paper.GetComponent<Renderer>().materials = paperMarkings;
            mixCooldown = addToMixCooldown;
            if (correctMixturesCompleted == 5)
            {
                buttonMix = true;
            }
            
        }

        else if (liquidID != mixtureNeededID)
        {
            correctMixturesCompleted = 0;
            for (int i = 1; i < paper.GetComponent<MeshRenderer>().materials.Length; i++)
            {
                paperMarkings[i] = paperNullMarking;
            }
            paper.GetComponent<Renderer>().materials = paperMarkings;
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

    public override void RecieveNetworkMessage(string recievedMessage)
    {
        throw new System.NotImplementedException();
    }

    public override void SendNetworkMessage(string messageToSend)
    {
        throw new System.NotImplementedException();
    }

    public override void SetID(int newID)
    {
        throw new System.NotImplementedException();
    }
}