using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class ButtonPressing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ButtonBehavior>() == true)
        {
            other.gameObject.GetComponent<Outline>().enabled = true;
            other.gameObject.GetComponent<ButtonBehavior>().ButtonPressed();
        }
    }
}
