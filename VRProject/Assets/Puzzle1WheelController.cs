using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1WheelController : MonoBehaviour
{
    public Vector3 Wheelposition;
    public float WheelRotateX;
    public float WheelRotateY;
    // Start is called before the first frame update

    public GameObject grabbedBy;
    public bool grabbed;

    public float angle;

    void Start()
    {
        Wheelposition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbed)
        {
            Debug.Log("Hand position prior = " + grabbedBy.transform.position);

            Vector3 handPosition = grabbedBy.transform.position;

            Debug.Log("Hand position after = " + handPosition);

            Vector3 handPositionResetXZ = new Vector3(handPosition.x, handPosition.y, 0);

            Debug.Log("Hand position reset = " + handPositionResetXZ);

            Vector3 handDirection = transform.position - handPositionResetXZ;

            Debug.Log("Hand direction prior = " + handDirection);

            handDirection.z = 0;

            Debug.Log("Hand position midway = " + handDirection);

            handDirection.Normalize();

            Debug.Log("Hand direction after normalize = " + handDirection);

            Debug.Log("Hand forward prior = " + transform.forward);

            transform.right = handDirection;

            Debug.Log("Hand forward after = " + transform.forward);
            //angle = Vector3.SignedAngle(transform.position, handDirection, transform.position);
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
         {
            grabbedBy = null;
            grabbed = false;
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
