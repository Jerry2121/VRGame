using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalReceptorBehavior : MonoBehaviour
{
    public GameObject crystalRecieved;

    private void OnTriggerExit(Collider other)
    {
        if (other == crystalRecieved)
        {
            crystalRecieved = null;
        }
    }
}
