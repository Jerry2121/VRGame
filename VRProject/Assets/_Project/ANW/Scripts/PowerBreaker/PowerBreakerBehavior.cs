using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBreakerBehavior : MonoBehaviour
{
    public GameObject[] buttonsNumeric;
    public int[] buttonIDsNumeric;
    public GameObject[] buttonsColored;
    public int[] buttonIDsColored;

    public int correctMatchesCompleted;

    public GameObject numericButtonPressed;
    public int numericButtonPressedID;
    public GameObject coloredButtonPressed;
    public int coloredButtonPressedID;

    public Material[] buttonLightsMaterials;
    public GameObject[] buttonLights;
    public Color32[] buttonLightsColors;

    public void CheckButtonMatch()
    {
        if (numericButtonPressedID == coloredButtonPressedID)
        {
            correctMatchesCompleted++;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(numericButtonPressed.transform.position, coloredButtonPressed.transform.position);

            numericButtonPressed.GetComponent<ButtonBehavior>().matchFound = true;
            numericButtonPressed = null;

            coloredButtonPressed.GetComponent<ButtonBehavior>().matchFound = true;
            coloredButtonPressed = null;
        }

        else if (numericButtonPressedID != coloredButtonPressedID)
        {
            correctMatchesCompleted = 0;
            numericButtonPressed = null;
            coloredButtonPressed = null;
        }
    }

    public void PaintLights()
    {
        Material[] tempMaterials = gameObject.GetComponent<MeshRenderer>().materials;

        for (int i = 2; i < gameObject.GetComponent<MeshRenderer>().materials.Length - 1; i++)
        {
            int tempInt = buttonIDsColored[i - 2];
            gameObject.GetComponent<MeshRenderer>().materials[i] = buttonLightsMaterials[tempInt];
            buttonLights[i - 2].SetActive(true);
            buttonLights[i - 2].GetComponent<Light>().color = buttonLightsColors[tempInt];
        }

        for (int i = 0; i < buttonsNumeric.Length; i++)
        {
            buttonsNumeric[i].GetComponent<ButtonBehavior>().buttonID = buttonIDsNumeric[i];
        }

        for (int i = 0; i < buttonsColored.Length; i++)
        {
            buttonsColored[i].GetComponent<ButtonBehavior>().buttonID = buttonIDsColored[i];
        }

        gameObject.GetComponent<MeshRenderer>().materials = tempMaterials;
    }
}
