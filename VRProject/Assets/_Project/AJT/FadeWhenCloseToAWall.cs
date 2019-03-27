using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeWhenCloseToAWall : MonoBehaviour
{
    public GameObject Player;
    public GameObject LeftUp;
    public GameObject LeftDown;
    public GameObject RightUp;
    public GameObject RightDown;
    public GameObject Image2;
    private bool HitFront;
    private bool HitBack;
    private bool HitLeft;
    private bool HitRightUp;
    private bool HitRight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // Bit shift the index of the layer (11) to get a bit mask
        int layerMask = 11;
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        /*RaycastHit hit;


        //Raycast to the front of the camera
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1, layerMask))
        {

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            Image2.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(255.0f / hit.distance));
            HitFront = true;
            return;
        }*/

        /*
        else if (HitFront == true && HitBack == false && HitLeft == false && HitRight == false && HitRightUp == false)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Image2.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            HitFront = false;
        }*/


        //Image2.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

        /*
        //Raycast to the back of the camera
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, 0.7f, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hit.distance, Color.yellow);
            Image2.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(255.0f / hit.distance));
            HitBack = true;
            return;

        }
        else if (HitFront == false && HitBack == true && HitLeft == false && HitRight == false && HitRightUp == false)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * 1000, Color.white);
          //  Image2.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            HitBack = false;
        }

        //Raycast to the left of the camera
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 0.7f, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit.distance, Color.yellow);
            Image2.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(255.0f / hit.distance));
            HitLeft = true;
            return;
        }
        else if (HitFront == false && HitBack == false && HitLeft == true && HitRight == false && HitRightUp == false)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 1000, Color.white);
           // Image2.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            HitLeft = false;
        }

        //Raycast to the right of the camera
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 0.7f, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
            Image2.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(255.0f / hit.distance));
            HitRight = true;
            return;
        }
        else if (HitFront == false && HitBack == false && HitLeft == false && HitRight == true && HitRightUp == false)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 1000, Color.white);
          //  Image2.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            HitRight = false;
        }




        //Raycast to the right up of the camera
        if (Physics.Raycast(RightUp.transform.position, RightUp.transform.TransformDirection(Vector3.forward), out hit, 0.7f, layerMask))
        {
            Debug.DrawRay(RightUp.transform.position, RightUp.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Image2.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(255.0f / hit.distance));
            HitRightUp = true;
        }
        else if (HitRightUp == true)
        {
            Debug.DrawRay(RightUp.transform.position, RightUp.transform.TransformDirection(Vector3.forward) * 1000, Color.green);
            Image2.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            HitRightUp = false;
        }

        */





        //int layerMask = 11;
        layerMask = ~layerMask;
        RaycastHit hit;
        Vector3 p1 = Player.transform.position;

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        
        if (Physics.SphereCast(p1, 2.0f, Camera.main.transform.forward, out hit, 0.0f, layerMask))
        {
            Image2.GetComponent<Image>().color = new Color32(0, 0, 0, 1);//(byte)Mathf.RoundToInt(255.0f / hit.distance));
            Debug.Log("Hit");
            Debug.Log((byte)Mathf.RoundToInt(255.0f / hit.distance));
        }
        else
        {
            Image2.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Player.transform.position, 0.7f);
    }
}
    
