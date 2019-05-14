using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    public GameObject powerBreaker;
    public string buttonType;
    public int buttonID;
    public bool matchFound;

    public void ButtonPressed()
    {
        if (matchFound)
        {
            return;
        }

        if (buttonType == "Numeric")
        {
            powerBreaker.GetComponent<PowerBreakerBehavior>().numericButtonPressed = this.gameObject;
            powerBreaker.GetComponent<PowerBreakerBehavior>().numericButtonPressedID = buttonID;
        }

        else if (buttonType == "Colored")
        {
            powerBreaker.GetComponent<PowerBreakerBehavior>().coloredButtonPressed = this.gameObject;
            powerBreaker.GetComponent<PowerBreakerBehavior>().coloredButtonPressedID = buttonID;
        }
    }
}
