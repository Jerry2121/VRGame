using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1WheelController : MonoBehaviour
{
    public Vector3 Wheelposition;
    public float WheelRotateX;
    public float WheelRotateY;
    // Start is called before the first frame update
    void Start()
    {
        Wheelposition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "VRHands" && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            Vector3 handPosition = other.transform.position;
            Vector3 handPositionResetXZ = new Vector3(handPosition.x, handPosition.y, 0);
            Vector3 handDirection = transform.position - handPositionResetXZ;
            handDirection.z = 0;
            float angle = Vector3.SignedAngle(handDirection, transform.position, )
        }
    }
    
    private void LateUpdate()
    {
        this.transform.position = Wheelposition;
        Quaternion rot = transform.rotation;
        Vector3 foo = rot.eulerAngles;
        foo.x = WheelRotateX;
        foo.y = WheelRotateY;
        this.transform.rotation = Quaternion.Euler(foo);
    }

    public void FindHandDirection(float angle)
    {
        //transform.rotation = new Vector3(0, -90, angle);
    }
}
