using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject destroyedVersion;

    public void Break()
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
