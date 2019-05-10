using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBreakerBehavior : MonoBehaviour
{
    public int[] buttonIDsNumeric;
    public int[] buttonIDsColored;

    public int correctMatchesCompleted;

    public int numericButtonPressedID;
    public int coloredButtonPressedID;

    public void CheckButtonMatch()
    {
        if (numericButtonPressedID == coloredButtonPressedID)
        {
            correctMatchesCompleted++;
        }

        else if (numericButtonPressedID != coloredButtonPressedID)
        {
            correctMatchesCompleted = 0;
        }
    }
}
