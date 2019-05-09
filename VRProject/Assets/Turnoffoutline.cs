using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
[DisallowMultipleComponent]
public class Turnoffoutline : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Outline>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
