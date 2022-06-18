using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
    public Transform target = null;

    private Animator animator = null;

    [SerializeField]
    private float handRange = 1f;
    [SerializeField]
    private Transform handPosition = null;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Vector3.Distance(target.transform.position, handPosition.position) <= handRange)
        {
            target.transform.SetParent(handPosition);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.5f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.5f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, target.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, target.rotation);

        animator.SetLookAtPosition(target.position);
    }
}