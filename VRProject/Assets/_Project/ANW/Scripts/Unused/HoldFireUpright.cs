using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldFireUpright : MonoBehaviour
{
    public Vector3 uprightRotation;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(uprightRotation.x, uprightRotation.y, uprightRotation.z);
    }
}
