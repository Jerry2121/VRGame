using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    [SerializeField] bool rotate;
    [SerializeField] float randomRange = 15;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (rotate)
        {
            transform.Rotate(new Vector3(Random.Range(0, randomRange), Random.Range(0, randomRange), Random.Range(0, randomRange)));
        }
        else transform.rotation = Quaternion.identity;
    }
}
