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
            int tempInt = buttonIDsColored[i];
            gameObject.GetComponent<MeshRenderer>().materials[i] = buttonLightsMaterials[tempInt];
            buttonLights[i - 1].SetActive(true);
            buttonLights[i - 1].GetComponent<Light>().color = buttonLightsColors[tempInt];
        }
    }
}
