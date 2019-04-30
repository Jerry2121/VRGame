using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public float MaxY;
    public float MinY;
    public GameObject grabbedBy;
    public bool grabbed;
    Vector3 LevelPos;
    // Start is called before the first frame update
    void Start()
    {
        LevelPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            grabbedBy = null;
            grabbed = false;
        }
        if (this.transform.position.y >= MaxY)
        {
            this.transform.position = new Vector3(LevelPos.x, MaxY, LevelPos.z);
        }
        if (this.transform.position.y <= MinY)
        {
            this.transform.position = new Vector3(LevelPos.x, MinY, LevelPos.z);
        }
        if (grabbed)
        {
            //LevelPos = this.transform.position;
            this.transform.position = new Vector3(LevelPos.x, grabbedBy.transform.position.y, LevelPos.z);
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
