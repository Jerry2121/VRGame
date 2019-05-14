using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressing : MonoBehaviour
{
    public Transform HandOffset;

    private void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(HandOffset.position, HandOffset.TransformDirection(Vector3.forward), out hit, 0.1f))
        {
            if (hit.collider.gameObject.GetComponent<ButtonBehavior>() == true)
            {
                hit.collider.gameObject.GetComponent<ButtonBehavior>().ButtonPressed();
            }
        }
    }
}
