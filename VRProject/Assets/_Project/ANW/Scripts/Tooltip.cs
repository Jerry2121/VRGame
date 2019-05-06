using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public GameObject ToolTipCanvas;
    // public TextMesh tooltip;
    // public TextMesh hint;

    // public void EnableUI() {
    // tooltip.SetActive = true;
    // if (hints are enabled) { hint.SetActive = true; } }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToolTipCanvas.SetActive(!ToolTipCanvas.activeSelf);
        }
    }
}
