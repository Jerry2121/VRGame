using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class BreakableObject : MonoBehaviour
{
    public GameObject destroyedVersion;
    private bool ran = false;

    public void Break()
    {
        if (!ran)
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            ran = true;
            Destroy(this.gameObject);
        }
    }
}
