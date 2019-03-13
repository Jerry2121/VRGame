using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OVRGrabbable))]
public class DisableHeld : MonoBehaviour
{
    //[SerializeField]
    OVRGrabbable ovrGrabbable;
    //[SerializeField]
    Rigidbody rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        ovrGrabbable = GetComponent<OVRGrabbable>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ovrGrabbable.isGrabbed && rigidbody.useGravity)
        {
            rigidbody.useGravity = false;
        }
        else if(ovrGrabbable.isGrabbed == false && rigidbody.useGravity == false)
        {
            rigidbody.useGravity = true;
        }
    }
}
