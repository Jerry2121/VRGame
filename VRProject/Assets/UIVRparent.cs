using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVRparent : MonoBehaviour
{
    public Transform VRPlayer;
    
    void Update()
    {
        this.gameObject.transform.position = VRPlayer.position;
        this.gameObject.transform.rotation = VRPlayer.rotation;
    }
}
