﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.OldNetworking
{

    [RequireComponent(typeof(Rigidbody))]
    public class TempPlayerMotor : MonoBehaviour
    {

        public Camera cam;

        private Vector3 velocity = Vector3.zero;
        private Vector3 rotation = Vector3.zero;
        private float cameraRotationX = 0f;
        private float currentCameraRotationX = 0f;

        [SerializeField]
        private float cameraRotationLimit = 85f;

        private Rigidbody rb;

        public bool canMove;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        //Runs every physics iteration
        void FixedUpdate()
        {
            if (canMove == false)
                return;

            PerformMovement();
            PerformRotation();
        }

        //Gets a movement vector
        public void Move(Vector3 _velocity)
        {
            velocity = _velocity;
        }

        //Gets a rotation vector
        public void Rotate(Vector3 _rotation)
        {
            rotation = _rotation;
        }

        //Gets a rotation vector for the camera
        public void RotateCamera(float _rotationX)
        {
            cameraRotationX = _rotationX;
        }

        //Move based on the velocity variable
        void PerformMovement()
        {
            if (velocity != Vector3.zero)
            {
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            }
        }

        //Rotate based on the rotation variable
        void PerformRotation()
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

            //Do cam rotation
            if (cam != null)
            {
                //Set rotation and clamp it
                currentCameraRotationX -= cameraRotationX;
                currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

                //Apply rotation to camera transform
                cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
            }
        }
    }
}