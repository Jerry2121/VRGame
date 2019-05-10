using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<BurnableObject>() == true)
        {
            other.gameObject.GetComponent<BurnableObject>().burnTimer += Time.deltaTime;
        }
    }
}
