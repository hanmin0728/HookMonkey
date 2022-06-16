using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 grapplePoint;
    [SerializeField]
    private LayerMask WhatIsGrappleable;
    private float maxDistance = 100f;
    public Transform gunTip, camera, player;
    private SpringJoint joint;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartGrapple();
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopGrapple();
        }
    }
    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartGrapple()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Physics.Raycast(ray, out hit, maxDistance, WhatIsGrappleable))
            {
                print("hit");
                grapplePoint = hit.point;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * 0.25f;

                joint.spring = 4.5f;
                joint.damper = 7f;
                joint.massScale = 4.5f;

                line.positionCount = 2;
            }
        }
      
    }
    void DrawRope()
    {
        if (line.positionCount == 0)
            return;
        //print(gunTip.position + " " + grapplePoint);
        line.SetPosition(0, gunTip.position);
        line.SetPosition(1, grapplePoint);
    }
    private void StopGrapple()
    {
        line.positionCount = 0;
        Destroy(joint);
    }

}
