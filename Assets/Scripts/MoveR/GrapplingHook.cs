using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public float maxDistance = 10000f;
    public LayerMask WhatIsGrappleable;
    [SerializeField]
    ForceMode forceMode;  
    public Transform target;
    LineRenderer line;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Grap();
        }
    }
    private void FixedUpdate()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, target.position);
    }
    public void Grap()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000f, Color.black, maxDistance);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, WhatIsGrappleable))
        {
            line.positionCount = 2;
        }
        gameObject.SetActive(true);
        target.transform.position = hit.point;
        transform.LookAt(target);
        transform.localScale = new Vector3(1, 1, 1);
    }

}
