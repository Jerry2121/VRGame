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
                gameObject.GetComponent<Rigidbody>().useGravity = true;
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
                other.gameObject.GetComponent<CrystalReceptorBehavior>().crystalRecieved = this.gameObject;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.transform.position = other.gameObject.transform.position;
                gameObject.transform.rotation = other.gameObject.transform.rotation;
            }
        }
    }
}
