using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using cakeslice;

public class TooltipBehavior : MonoBehaviour
{
    public GameObject InteractionIndicator;
    public GameObject ToolTipCanvas;
    public GameObject ToolTipGameObject;
    public GameObject HintGameObject;
    public GameObject HintDesc;
    public GameObject LeftHandAnchor;
    public GameObject RightHandAnchor;
    private GameObject ToolTipName;
    private GameObject ToolTipDesc;
    private GameObject ToolTipGrabbable;
    public static bool HintActive;
    public static bool ToolTipActive;
    public static bool interactL = false;
    public static bool interactR = false;
    public Transform LeftHandOffset;
    public Transform RightHandOffset;

    private void Start()
    {
        InteractionIndicator = GameObject.Find("InteractionIndicator");
        ToolTipCanvas = GameObject.Find("ToolTipCanvas");
        ToolTipName = GameObject.Find("ToolTipName");
        ToolTipDesc = GameObject.Find("ToolTipDesc");
        ToolTipGrabbable = GameObject.Find("ToolTipGrabbable");
    }
    // do a raycast
    // if (hit.gameObject.GetComponent<Tooltip>() == true)
    // { hit.gameObject.GetComponent<Tooltip>() }
    private void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5))
        {
            if (hit.collider.gameObject.GetComponent<Tooltip>() == true)
            {
                InteractionIndicator.transform.position = hit.collider.gameObject.transform.position + new Vector3(0, 1.3f, 0);
                InteractionIndicator.GetComponent<LookAtPlayer>().LookNow();
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if(name == "LeftHandAnchor")
                {
                    interactL = true;
                }
                if(name == "RightHandAnchor")
                {
                    interactR = true;
                }
            }
            else
            {
                if (name == "LeftHandAnchor")
                {
                    interactL = false;
                }
                if (name == "RightHandAnchor")
                {
                    interactR = false;
                }
                if (interactL)
                {
                    RightHandAnchor.GetComponent<TooltipBehavior>().enabled = false;
                }
                if (interactR)
                {
                    LeftHandAnchor.GetComponent<TooltipBehavior>().enabled = false;
                }
                if(!interactL && !interactR)
                {
                    InteractionIndicator.transform.position = hit.collider.gameObject.transform.position + new Vector3(0, 20, 0);
                    LeftHandAnchor.GetComponent<TooltipBehavior>().enabled = true;
                    RightHandAnchor.GetComponent<TooltipBehavior>().enabled = true;
                }
                return;
            }
            if (hit.collider.gameObject.GetComponent<Tooltip>() == true && OVRInput.GetUp(OVRInput.RawButton.X) && !ToolTipActive)
            {
                ToolTipName.GetComponent<TextMeshProUGUI>().text = "Name: " + hit.collider.gameObject.GetComponent<Tooltip>().name;
                ToolTipDesc.GetComponent<TextMeshProUGUI>().text = "Description: " + hit.collider.gameObject.GetComponent<Tooltip>().Description;
                if (hit.collider.gameObject.GetComponent<Tooltip>().Grabbable == true)
                {
                    ToolTipGrabbable.GetComponent<TextMeshProUGUI>().text = "Grabbable: True";
                }
                else
                {
                    ToolTipGrabbable.GetComponent<TextMeshProUGUI>().text = "Grabbable: False";
                }
                hit.collider.gameObject.GetComponent<Outline>().enabled = true;
                ToolTipCanvas.GetComponent<Canvas>().enabled = !ToolTipCanvas.GetComponent<Canvas>().enabled;
                ToolTipActive = true;
            }
            else if (ToolTipActive && OVRInput.GetUp(OVRInput.RawButton.X) && !HintActive)
            {
                Debug.Log("foo");
                ToolTipCanvas.GetComponent<Canvas>().enabled = false;
                hit.collider.gameObject.GetComponent<Outline>().enabled = false;
                ToolTipActive = false;
            }
            if (hit.collider.gameObject.GetComponent<Tooltip>() == true && OVRInput.GetUp(OVRInput.RawButton.Y) && !HintActive)
            {
                HintActive = true;
                ToolTipGameObject.SetActive(false);
                HintGameObject.SetActive(true);
                HintDesc.GetComponent<TextMeshProUGUI>().text = "Description: " + hit.collider.gameObject.GetComponent<Tooltip>().HintDescription;
            }
            else if (OVRInput.GetUp(OVRInput.RawButton.Y) && HintActive)
            {
                HintGameObject.SetActive(false);
                ToolTipGameObject.SetActive(true);
                HintActive = false;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }
}
