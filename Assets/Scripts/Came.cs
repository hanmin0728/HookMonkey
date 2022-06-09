using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Came : MonoBehaviour
{
    public Transform objTarget = null;

    public float distance = 6.0f;
    public float height = 1.75f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    void ThirdCamera()
    {
        float objTargetRotationAngle = objTarget.eulerAngles.y;
        float objHeight = objTarget.position.y + height;
        float nowRotationAngle = transform.eulerAngles.y;
        float nowHeight = transform.position.y;

        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, rotationDamping * Time.deltaTime);

        nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamping * Time.deltaTime);

        Quaternion nowRotation = Quaternion.Euler(0f, nowRotationAngle, 0f);

        transform.position = objTarget.position;
        transform.position -= nowRotation * Vector3.forward * distance;

        transform.position = new Vector3(transform.position.x, nowHeight, transform.position.z);

        transform.LookAt(objTarget);
    }

    private void LateUpdate()
    {
        ThirdCamera();
    }
}
