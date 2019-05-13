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

    public int numericButtonPressedID;
    public int coloredButtonPressedID;

    public Material[] buttonLightsMaterials;
    public GameObject[] buttonLights;
    public Color32[] buttonLightsColors;

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

    public void PaintLights()
    {
        Material[] tempMaterials = gameObject.GetComponent<MeshRenderer>().materials;

        for (int i = 1; i < gameObject.GetComponent<MeshRenderer>().materials.Length - 1; i++)
        {
            int tempInt = buttonIDsColored[i - 1];
            gameObject.GetComponent<MeshRenderer>().materials[i] = buttonLightsMaterials[tempInt];
            buttonLights[i - 1].SetActive(true);
            buttonLights[i - 1].GetComponent<Light>().color = buttonLightsColors[tempInt];
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
