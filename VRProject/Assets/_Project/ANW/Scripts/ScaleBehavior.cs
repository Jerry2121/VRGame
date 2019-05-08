using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBehavior : MonoBehaviour
{
    public Vector3 originalPosition;
    public float weightHeld;
    public GameObject partnerScale;
    public float pushDownDistance;
    public float pushDown;
    public float maxPushDown;
    public float levelComparisonBalance;
    [HideInInspector] public bool pressurePlatePressed;
    public bool adminScale;
    bool puzzleComplete;
    public GameObject prize;
    public Transform prizeSpawnLocation;
    public bool prizeSpawned;

    public bool testThingy;

    private void Start()
    {
        originalPosition = gameObject.transform.position;
    }

    void Update()
    {
        if (prizeSpawned)
        {
            return;
        }

        if (puzzleComplete && adminScale)
        {
            Instantiate(prize, prizeSpawnLocation);
            prizeSpawned = true;
            partnerScale.GetComponent<ScaleBehavior>().prizeSpawned = true;
        }

        pushDown = Mathf.Lerp(0.0f, (weightHeld - partnerScale.GetComponent<ScaleBehavior>().weightHeld / levelComparisonBalance), 0.005f);
        if(pushDown == 0)
        {
            pushDown = -Mathf.Lerp(0.0f, originalPosition.y - transform.position.y, 0.005f);
        }

        pushDownDistance = weightHeld - partnerScale.GetComponent<ScaleBehavior>().weightHeld;

        Debug.Log("position - maxPushDown = " + (originalPosition.y - maxPushDown));
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - pushDown, transform.position.z);



        if (gameObject.transform.position.y <= originalPosition.y - maxPushDown)
        {
            gameObject.transform.position = new Vector3(originalPosition.x, originalPosition.y - maxPushDown, originalPosition.z);
        }

        else if (gameObject.transform.position.y >= originalPosition.y + maxPushDown)
        {
            gameObject.transform.position = new Vector3(originalPosition.x, originalPosition.y + maxPushDown, originalPosition.z);
        }

        else if (gameObject.transform.position.y <= originalPosition.y - pushDownDistance)
        {
            gameObject.transform.position = new Vector3(originalPosition.x, originalPosition.y - pushDownDistance, originalPosition.z);
        }

        else if (gameObject.transform.position.y >= originalPosition.y + pushDownDistance)
        {
            gameObject.transform.position = new Vector3(originalPosition.x, originalPosition.y + pushDownDistance, originalPosition.z);
        }


        if (!pressurePlatePressed && partnerScale.GetComponent<ScaleBehavior>().pressurePlatePressed)
        {
            puzzleComplete = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "PressurePlate")
        {
            pressurePlatePressed = true;
        }

        if (other.gameObject.GetComponent<ObjectBehavior>() == true && !other.gameObject.GetComponent<ObjectBehavior>().weightApplied && !other.gameObject.GetComponent<OVRGrabbable>().isGrabbed)
        {
                weightHeld += other.gameObject.GetComponent<ObjectBehavior>().objectWeight;
                other.gameObject.GetComponent<ObjectBehavior>().weightApplied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PressurePlate")
        {
            pressurePlatePressed = false;
        }

        if (other.gameObject.GetComponent<ObjectBehavior>() == true && other.gameObject.GetComponent<ObjectBehavior>().weightApplied)
        {
            weightHeld -= other.gameObject.GetComponent<ObjectBehavior>().objectWeight;
            other.gameObject.GetComponent<ObjectBehavior>().weightApplied = false;
        }
    }
}
