using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    float burnTimer;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Burnable")
        {
            burnTimer += Time.deltaTime;
            if (burnTimer >= 3.0f)
            {
                other.gameObject.GetComponent<BurnableObject>().Burn();
            }
        }
    }
}
