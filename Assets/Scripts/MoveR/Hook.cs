//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Hook : MonoBehaviour
//{
//    private LineRenderer _lineRenderer;
//    private Vector3 _grapplePoint;
//    public LayerMask whatIsGrappling;
//    public Transform gunTip, camera, player;
//    private float maxDistance = 100f;
//    private SpringJoint joint;
//    private void Awake()
//    {
//        _lineRenderer = GetComponent<LineRenderer>();
//       // _lineRenderer.useWorldSpace = false;
//    }
//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(1))
//        {
//            StartGrapple();
//        }
//        else if (Input.GetMouseButtonUp(1))
//        {
//            StopGrapple();
//        }
//    }
//    private void LateUpdate()
//    {
//        DrawRope();
//    }
//    private void StartGrapple()
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, whatIsGrappling))
//        {
//            _grapplePoint = hit.point;
//            joint = player.gameObject.AddComponent<SpringJoint>();
//            joint.autoConfigureConnectedAnchor = false;
//            joint.connectedAnchor = _grapplePoint;


//            float distanceFromPoint = Vector3.Distance(player.position, _grapplePoint);

//            joint.maxDistance = distanceFromPoint * 0.8f;
//            joint.minDistance = distanceFromPoint * 0.25f;

//            joint.spring = 4.5f;
//            joint.damper = 7f;
//            joint.massScale = 4.5f;

//            _lineRenderer.positionCount = 2;
//        }
//    }
//    void DrawRope()
//    {
//        _lineRenderer.SetPosition(0, gunTip.position);
//        _lineRenderer.SetPosition(1, _grapplePoint);
//    }
//    private void StopGrapple()
//    {
//        _lineRenderer.positionCount = 2;
//        Destroy(joint); 
//    }

  
//}
