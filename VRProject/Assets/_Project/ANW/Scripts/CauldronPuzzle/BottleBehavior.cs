using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBehavior : MonoBehaviour
{
    public int liquidID;
    public Transform bottleTop;
    public Transform bottleBottom;
    public Transform bottleReset;

    public GameObject cauldronFill;

    private void Update()
    {
        if (bottleBottom.transform.position.y > bottleTop.transform.position.y)
        {
            Debug.Log("The bottle is spilling.");
            RaycastHit hit;
            if (Physics.Raycast(bottleTop.position, Vector3.down, out hit))
            {
                Debug.DrawRay(bottleTop.position, Vector3.down, Color.red);
                if (hit.collider.tag == "CauldronFill")
                {
                    Debug.Log("HIT!");
                    cauldronFill.GetComponent<CauldronBehavior>().CheckMixture(liquidID);
                }
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {        
        if (collision.gameObject.tag == "CauldronFill")
        {
            Debug.Log("The bottle doesn't go in the cauldron.");
            gameObject.transform.position = bottleReset.position;
            gameObject.transform.rotation = bottleReset.rotation;
        }
    }
}