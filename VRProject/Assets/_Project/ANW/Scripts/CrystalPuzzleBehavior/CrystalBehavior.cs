using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBehavior : MonoBehaviour
{
    float recentlyGrabbedTimer;
    bool recentlyGrabbed;

    private void Update()
    {
        if (gameObject.GetComponent<OVRGrabbable>().isGrabbed)
        {
            recentlyGrabbedTimer = 1.0f;
        }

        else if (!gameObject.GetComponent<OVRGrabbable>().isGrabbed)
        {
            if (recentlyGrabbedTimer > 0)
            {
                recentlyGrabbed = true;
                recentlyGrabbedTimer -= Time.deltaTime;
            }

            else if (recentlyGrabbedTimer < 0)
            {
                recentlyGrabbed = false;
                recentlyGrabbedTimer = 0;
            }
        }
    }

   void OnTriggerStay(Collider other)
    {
        {
            if (other.gameObject.GetComponent<CrystalReceptorBehavior>() && recentlyGrabbed)
            {
                GameObject g = Instantiate(gameObject, other.gameObject.transform);
                other.gameObject.GetComponent<CrystalReceptorBehavior>().crystalRecieved = g;

                Destroy(this.gameObject);
            }
        }
    }
}
