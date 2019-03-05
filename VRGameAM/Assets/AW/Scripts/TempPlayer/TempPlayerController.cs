﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TempPlayerMotor))]
public class TempPlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private LayerMask environmentMask;

    [Header("Spring settings")]
    [SerializeField]
    private float jointSpring = 20;
    [SerializeField]
    private float jointMaxForce = 40;

    //component caching
    private TempPlayerMotor motor;

    void Start()
    {
        motor = GetComponent<TempPlayerMotor>();
    }

    void Update()
    {
        /*if (PauseMenu.isOn || PlayerUI.InInventory)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            return;
        }*/
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //Calculate movement velocity as 3D vector
        float _xMove = Input.GetAxis("Horizontal");
        float _zMove = Input.GetAxis("Vertical");

        Vector3 _moveHorizontal = transform.right * _xMove;
        Vector3 _moveVertical = transform.forward * _zMove;

        //Final movement vector
        Vector3 _velocity = (_moveHorizontal + _moveVertical) * speed;

        //Apply movement
        motor.Move(_velocity);

        //Calculate rotation as a 3D vector for turning
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) /** PlayerPrefs.GetFloat("XSensitivity")*/;

        //Apply rotation
        motor.Rotate(_rotation);

        //Calculate camera rotation as a 3D vector for loking up or down
        float _xRot = Input.GetAxisRaw("Mouse Y");
        
        
            float _cameraRotationX = _xRot /** PlayerPrefs.GetFloat("YSensitivity")*/;

            //Apply camera rotation
            motor.RotateCamera(_cameraRotationX);

    }
}
