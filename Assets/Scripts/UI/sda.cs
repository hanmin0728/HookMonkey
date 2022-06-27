using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class sda : MonoBehaviour
{
    public Transform cam;
    private void LateUpdate()
    {
        transform.LookAt(cam);
    }
}
