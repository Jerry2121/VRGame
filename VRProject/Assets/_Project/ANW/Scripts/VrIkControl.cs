using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class VrIkControl : MonoBehaviour
{
    protected Animator animator;

    public Transform rightHandObj;
    public Transform leftHandObj;

    public Transform rightFoot;
    public Transform leftFoot;
    public Transform rightKnee;
    public Transform leftKnee;

    public Transform characterHead;
    public Transform characterSpine;

    public Transform ovrCamera;
    public Transform trackingSpace;
    public float playerYOffset;
    public float playerXOffset;
    public float playerZOffset;
    public float playerZOffsetCrouch;
    public float trackingSpaceReq;

    public bool characterMove;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterMove = false;
    }

    void Update()
    {
        gameObject.transform.position = ovrCamera.transform.position - new Vector3(0, playerYOffset, playerZOffset);

        if (characterHead.transform.position.y < 1.375)
        {
            gameObject.transform.position = new Vector3(0, -0.4f, 0);
        }
    }

    void LateUpdate()
    {

        characterSpine.eulerAngles = new Vector3 (0, ovrCamera.rotation.x + 90, -90);
        //characterSpine.rotation = ovrCamera.rotation;
    }

    // A callback for calculating IK
    void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);

        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);

        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);

        // If the players foot is lower than the ground, position back up.
        // This will stop the tracking spaces movement from clipping the player through the ground.
        if (rightFoot.position.y < 0.1f)
        {
            animator.SetIKPosition(AvatarIKGoal.RightFoot, new Vector3 (rightFoot.position.x, 0.1100572f, rightFoot.position.z));
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, new Vector3 (leftFoot.position.x, 0.1100572f, leftFoot.position.z));
           
            Vector3 foo = rightFoot.transform.rotation.eulerAngles;
            foo = -rightKnee.transform.rotation.eulerAngles;
            foo.z += 105;
            foo.y -= 60;
            if (characterMove)
            {
                foo.y -= 30;
            }
            rightFoot.transform.rotation = Quaternion.Euler(foo);
            //animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.Euler(foo));

            Vector3 foo2 = leftFoot.transform.rotation.eulerAngles;
            foo2 = -leftKnee.transform.rotation.eulerAngles;
            foo2.z += 90;
            foo2.y -= 90;
            leftFoot.transform.rotation = Quaternion.Euler(foo);
            //animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.Euler(foo));
        }

        if (rightFoot.position.y > 0.1f)
        {
            // I DON'T KNOW
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (trackingSpace.position.y - playerYOffset), gameObject.transform.position.z);
        }
    }
}
