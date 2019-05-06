using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerBehavior : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BreakableObject>() == true)
        {
            other.gameObject.GetComponent<BreakableObject>().Break();
        }
    }
}
