// --------------------------------------
// This script is totally optional. It is an example of how you can use the
// destructible versions of the objects as demonstrated in my tutorial.
// Watch the tutorial over at http://youtube.com/brackeys/.
// --------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {
    public GameObject grabbedBy;
    public bool grabbed;
    public GameObject destroyedVersion;	// Reference to the shattered version of the object

	// If the player clicks on the object
	void OnMouseDown ()
	{
		// Spawn a shattered object
		Instantiate(destroyedVersion, transform.position, transform.rotation);
		// Remove the current object
		Destroy(gameObject);
	}

    private void Update()
    {
        if (grabbed)
        {
            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
            {
                grabbedBy = null;
                grabbed = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != grabbedBy && grabbed)
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "VRHands" && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            grabbedBy = other.gameObject;
            grabbed = true;
        }
    }
}
