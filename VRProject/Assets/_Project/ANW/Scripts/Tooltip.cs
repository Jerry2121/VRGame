using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
[DisallowMultipleComponent]
[RequireComponent(typeof(Outline))]
public class Tooltip : MonoBehaviour
{
    public string Name;
    public bool Grabbable;
    [Header("Max 64 Characters (INCLUDES SPACES)")]
    public string Description;
    // public TextMesh tooltip;
    // public TextMesh hint;

    // public void EnableUI() {
    // tooltip.SetActive = true;
    // if (hints are enabled) { hint.SetActive = true; } }
    //Max 64 Characters (INCLUDES SPACES)
    private void Update()
    {

    }
}
