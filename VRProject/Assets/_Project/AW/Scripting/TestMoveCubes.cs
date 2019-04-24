using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveCubes : MonoBehaviour
{
    [SerializeField]
    bool moveLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("FOO");
        MoveCube.Move(true, moveLeft);
    }

    private void OnTriggerExit(Collider other)
    {
        MoveCube.Move(false, moveLeft);
    }

}
