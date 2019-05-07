using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipBehavior : MonoBehaviour
{
    // do a raycast
    // if (hit.gameObject.GetComponent<Tooltip>() == true)
    // { hit.gameObject.GetComponent<Tooltip>() }
    private void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, 10))
            print("There is something in front of the object!");
    }
}
