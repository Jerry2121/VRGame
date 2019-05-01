using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBehavior : MonoBehaviour
{
    public int liquidID;
    public Transform bottleTop;
    public Transform bottleBottom;

    public float addToPourCooldown;
    float pourCooldown;

    public GameObject cauldronFill;

    private void Start()
    {
        pourCooldown = 0f;
    }

    private void Update()
    {
        
        if (pourCooldown > 0)
        {
            pourCooldown -= Time.deltaTime;
            return;
        }

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
                    pourCooldown = 5f;
                }
            }
        }
    }
}