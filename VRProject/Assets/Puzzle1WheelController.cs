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
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "VRHands")
        {
            this.transform.position = Wheelposition;
            Quaternion rot = transform.rotation;
            Vector3 foo = rot.eulerAngles;
            foo.x = WheelRotateX;
            foo.y = WheelRotateY;
            this.transform.rotation = Quaternion.Euler(foo);
        }
    }
}
