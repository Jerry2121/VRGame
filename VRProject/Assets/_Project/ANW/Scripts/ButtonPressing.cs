using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class ButtonPressing : MonoBehaviour
{
    public Transform HandOffset;

    private void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(HandOffset.position, HandOffset.TransformDirection(Vector3.forward), out hit, 0.01f))
        {
            Debug.DrawRay(HandOffset.position, HandOffset.TransformDirection(Vector3.forward), Color.blue);
            if (hit.collider.gameObject.GetComponent<ButtonBehavior>() == true)
            {
                hit.collider.gameObject.GetComponent<Outline>().enabled = true;
                hit.collider.gameObject.GetComponent<ButtonBehavior>().ButtonPressed();
            }
        }
    }
}
