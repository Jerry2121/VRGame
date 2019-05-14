using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressing : MonoBehaviour
{
    public string whichHand;
    public Transform LeftHandOffset;
    public Transform RightHandOffset;

    private void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (whichHand == "leftHand")
        {
            if (Physics.Raycast(LeftHandOffset.position, LeftHandOffset.TransformDirection(Vector3.forward), out hit, 0.1f))
            {
                if (hit.collider.gameObject.GetComponent<ButtonBehavior>() == true)
                {
                    hit.collider.gameObject.GetComponent<ButtonBehavior>().ButtonPressed();
                }
            }
        }

        else if (whichHand == "rightHand")
        {
            if (Physics.Raycast(RightHandOffset.position, RightHandOffset.TransformDirection(Vector3.forward), out hit, 0.1f))
            {
                if (hit.collider.gameObject.GetComponent<ButtonBehavior>() == true)
                {
                    hit.collider.gameObject.GetComponent<ButtonBehavior>().ButtonPressed();
                }
            }
        }
    }
}
