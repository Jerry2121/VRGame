﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;

public class LeverScript : MonoBehaviour
{
    public float MaxY;
    public float MinY;
    public GameObject grabbedBy;
    public bool grabbed;
    Vector3 LevelPos;
    Vector3 LevelRot;
    Quaternion LeverRot;
    public bool leverActivated;
    public GameObject OtherLever;
    public GameObject EndGameCanvas;
    public GameObject WinObject;
    public GameObject LooseObject;
    public Camera CenterEyeCamera;
    public GameObject GameOverTitle;
    public GameObject GameOverClosing;
    private static bool ran;

    // Start is called before the first frame update
    void Start()
    {
        LevelPos = transform.position;
        LeverRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (EndGameCanvas == null)
        {
            if (NetworkingManager.GetLocalPlayer() != null)
            {
                EndGameCanvasScript canvasscript = NetworkingManager.GetLocalPlayer().GetComponentInChildren<EndGameCanvasScript>();
                EndGameCanvas = canvasscript.gameObject;
                WinObject = canvasscript.WinObject;
                LooseObject = canvasscript.LooseObject;
                CenterEyeCamera = canvasscript.CenterEyeCamera;
                GameOverTitle = canvasscript.GameOverTitle;
                GameOverClosing = canvasscript.GameOverClosing;
            }
            else
            {
                return;
            }
        }
        transform.rotation = LeverRot;
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
        {
            grabbedBy = null;
            grabbed = false;
        }
        if (grabbed)
        {
            //LevelPos = this.transform.position;
            this.transform.position = new Vector3(LevelPos.x, grabbedBy.transform.position.y, LevelPos.z);
            if (this.transform.position.y >= MaxY)
            {
                this.transform.position = new Vector3(LevelPos.x, MaxY, LevelPos.z);
            }
            if (this.transform.position.y <= MinY)
            {
                this.transform.position = new Vector3(LevelPos.x, MinY, LevelPos.z);
                leverActivated = true;
            }
            
        }
        //If Player 1 Pulls Lever and Wins
        if (leverActivated && this.gameObject.name == "1LeverHandle" && !OtherLever.GetComponent<LeverScript>().leverActivated && !ran)
        {
            GameOverClosing.SetActive(true);
            GameOverTitle.SetActive(true);
            WinObject.SetActive(true);
            LooseObject.SetActive(false);
            CenterEyeCamera.cullingMask = 1 << 12;
            ran = true;
        }
        //If Player 2 Pulls Lever and Wins
        if (leverActivated && this.gameObject.name == "2LeverHandle" && !OtherLever.GetComponent<LeverScript>().leverActivated && !ran)
        {
            GameOverClosing.SetActive(true);
            GameOverTitle.SetActive(true);
            WinObject.SetActive(true);
            LooseObject.SetActive(false);
            CenterEyeCamera.cullingMask = 1 << 12;
            ran = true;
        }
        //If Player 2 Pulls Lever, Player 1 Looses
        if (!leverActivated && this.gameObject.name == "1LeverHandle" && OtherLever.GetComponent<LeverScript>().leverActivated && ran)
        {
            GameOverClosing.SetActive(true);
            GameOverTitle.SetActive(true);
            WinObject.SetActive(false);
            LooseObject.SetActive(true);
            CenterEyeCamera.cullingMask = 1 << 12;
        }
        //If Player 1 Pulls Lever, Player 2 Looses
        if (!leverActivated && this.gameObject.name == "2LeverHandle" && OtherLever.GetComponent<LeverScript>().leverActivated && ran)
        {
            GameOverClosing.SetActive(true);
            GameOverTitle.SetActive(true);
            WinObject.SetActive(false);
            LooseObject.SetActive(true);
            CenterEyeCamera.cullingMask = 1 << 12;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "VRHands" && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            grabbedBy = other.gameObject;
            grabbed = true;
        }
    }
}
