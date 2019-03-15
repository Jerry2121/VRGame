using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

[RequireComponent(typeof(VisualEffect))]
public class LaserTest : MonoBehaviour
{
    VisualEffect VFX;

    // Start is called before the first frame update
    void Start()
    {
        VFX = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        VFX.enabled = false;
        Debug.Log("Laser Hit " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        VFX.enabled = true;
    }

}
