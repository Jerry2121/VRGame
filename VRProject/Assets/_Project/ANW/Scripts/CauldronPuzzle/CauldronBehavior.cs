﻿using System.Collections;
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

    int[] previousMixtures = new int[5];

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

    public override NetworkObject m_NetworkObject { get; protected set; }
    public override int ID { get => m_ID; protected set => m_ID = value; }
    [HideInNormalInspector]
    [SerializeField]
    int m_ID;

    private void Start()
    {
        m_NetworkObject = GetNetworkObjectForObject(this.transform);
        RegisterSelf();

        correctMixturesCompleted = 0;
        mixCooldown = 0;
        markingLerping = false;
        markingLerp = 0;
        //mixtureNeededID = Random.Range(0, 6);
        //cauldronMarking.GetComponent<Renderer>().material.color = cauldronMarkingsArray[mixtureNeededID];
        paperMarkings = paper.GetComponent<Renderer>().materials;
        powerBreaker = GameObject.Find("PowerBreaker");

        if (NetworkingManager.s_Instance.IsConnected() && NetworkingManager.s_Instance.IsHost() == false)
            return;

        UpdateMixture(-1);

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
                powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsNumeric = powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored;
                for (int i = 0; i < powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored.Length; i++)
                {
                    int tmp = powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored[i];
                    int j = Random.Range(i, powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored.Length);
                    powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored[i] = powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored[j];
                    powerBreaker.GetComponent<PowerBreakerBehavior>().buttonIDsColored[j] = tmp;
                }
                powerBreaker.GetComponent<PowerBreakerBehavior>().PaintLights();

                buttonMix = false;
            }
        }

        fillLerp = (float)correctMixturesCompleted / correctMixturesNeeded;
        //Debug.Log(fillLerp);
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
        if (mixCooldown > 0 || mixtureFinished)
        {
            return;
        }

        if (NetworkingManager.s_Instance.IsConnected() && NetworkingManager.s_Instance.IsHost() == false)
            return;

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

    void UpdateMixture(int liquidID)
    {
        mixtureNeededID = Random.Range(0, 6);
        
        if (mixtureNeededID == liquidID)
        {
            UpdateMixture(liquidID);
            return;
        }

        SendNetworkMessage(NetworkTranslater.CreatePuzzleProgressMessage(NetworkingManager.ClientID(), m_NetworkObject.m_ObjectID, m_ID, mixtureNeededID));

        markingLerping = true;
    }

    public override void RecieveNetworkMessage(string recievedMessage)
    {
        if(NetworkTranslater.TranslatePuzzleProgressMessage(recievedMessage, out int clientID, out int objectID, out int componentID, out int numOne))
        {
            mixtureNeededID = numOne;
            markingLerping = true;
        }
    }

    public override void SendNetworkMessage(string messageToSend)
    {
        NetworkingManager.s_Instance.SendNetworkMessage(messageToSend);
    }

    public override void SetID(int newID)
    {
        if (newID > -1)
        {
            if (Debug.isDebugBuild)
                Debug.Log(string.Format("CauldronBehaviour -- SetID: ID set to {0}", newID));
            ID = newID;
        }
    }

    public override void RegisterSelf()
    {
        base.RegisterSelf();
    }

    [ExecuteAlways]
    public override void SetNetworkObject(NetworkObject newNetworkObject)
    {
        base.SetNetworkObject(newNetworkObject);
    }

    public override void Reset()
    {
        base.Reset();
    }
}