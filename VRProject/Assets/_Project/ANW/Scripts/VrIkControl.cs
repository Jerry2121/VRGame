﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class VrIkControl : MonoBehaviour
{
    protected Animator animator;

    public Transform rightHandObj;
    public Transform leftHandObj;
    public Transform ovrCamera;
    public Transform trackingSpace;
    public Transform headObj;
    public Vector3 playerOffset;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        this.gameObject.transform.position = ovrCamera.transform.position - playerOffset;
        headObj.transform.rotation = trackingSpace.transform.rotation;
    }

    // A callback for calculating IK
    void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);

        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
    }
}
