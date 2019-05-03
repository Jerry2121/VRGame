using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Burnable")
        {
            other.gameObject.GetComponent<BurnableObject>().burnTimer += Time.deltaTime;
        }
    }
}
