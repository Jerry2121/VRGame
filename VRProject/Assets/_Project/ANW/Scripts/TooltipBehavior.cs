using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using cakeslice;

public class TooltipBehavior : MonoBehaviour
{
    public GameObject InteractionIndicator;
    public GameObject ToolTipCanvas;
    private GameObject ToolTipName;
    private GameObject ToolTipDesc;
    private GameObject ToolTipGrabbable;
    public static bool interactL = false;
    public static bool interactR = false;
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
                if(!interactL && !interactR)
                    InteractionIndicator.transform.position = hit.collider.gameObject.transform.position + new Vector3(0, 20, 0);
                return;
            }
            if (hit.collider.gameObject.GetComponent<Tooltip>() == true && OVRInput.GetUp(OVRInput.RawButton.X))
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
                hit.collider.gameObject.GetComponent<Outline>().enabled = !hit.collider.gameObject.GetComponent<Outline>().enabled;
                ToolTipCanvas.GetComponent<Canvas>().enabled = !ToolTipCanvas.GetComponent<Canvas>().enabled;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }
}
