using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1WheelController : MonoBehaviour
{
    //public GameObject wheel;
    //public Vector3 Wheelposition;
    //public float WheelRotateX;
    //public float WheelRotateY;
    // Start is called before the first frame update
    public int spins;

    public GameObject grabbedBy;
    public bool grabbed;

    //public float angle;
    //Vector3 forward;

    void Start()
    {
        //Wheelposition = this.transform.position;
        //forward = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbed)
        {
            //Debug.Log("Hand position prior = " + grabbedBy.transform.position);

            Vector3 handPosition = grabbedBy.transform.position;

            //Debug.Log("Hand position after = " + handPosition);

            Vector3 handPositionResetXZ = new Vector3(0, handPosition.y, handPosition.z);

            //Debug.Log("Hand position reset = " + handPositionResetXZ);

            Vector3 handDirection = transform.position - handPositionResetXZ;

            //Debug.Log("Hand direction prior = " + handDirection);

            handDirection.x = 0;

            //Debug.Log("Hand position midway = " + handDirection);

            handDirection.Normalize();

            //Debug.Log("Hand direction after normalize = " + handDirection);

            //Debug.Log("Hand forward prior = " + transform.forward);

            float rot = Vector3.SignedAngle(new Vector3(0,0,1), handDirection, new Vector3(1,0,0));
            Quaternion rotation = Quaternion.Euler(0, 270, -rot + 180 + 105);
            transform.rotation = rotation;
           // Debug.Log("Dir: " + handDirection);
           // Debug.Log("Angle = " + rot);
            //transform.LookAt(transform.position + handDirection);
            //transform.right = handDirection;

            //Debug.Log("Hand forward after = " + transform.forward);
            //angle = Vector3.SignedAngle(transform.position, handDirection, transform.position);
        }
        
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
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
