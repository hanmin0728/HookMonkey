using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public Transform _target;
    public float detailX = 5f;
    public float detailY = 5f;
    public float height = 1.8f;
    public float rotateX;
    public float rotateY;
    //    public Transform objTarget = null;

    //    public float distance = 6.0f;
    //    public float height = 1.75f;
    //    public float heightDamping = 2.0f;
    //    public float rotationDamping = 3.0f;

    //void ThirdCamera()
    //{
    //    float objTargetRotationAngle = objTarget.eulerAngles.y;
    //    float objHeight = objTarget.position.y + height;
    //    float nowRotationAngle = transform.eulerAngles.y;
    //    float nowHeight = transform.position.y;

    //    nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, rotationDamping * Time.deltaTime);

    //    nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamping * Time.deltaTime);

    //    Quaternion nowRotation = Quaternion.Euler(0f, nowRotationAngle, 0f);

    //    transform.position = objTarget.position;
    //    transform.position -= nowRotation * Vector3.forward * distance;

    //    transform.position = new Vector3(transform.position.x, nowHeight, transform.position.z);

    //    transform.LookAt(objTarget);
    //}


    //public Transform objToFollow;
    //public float followSpd = 10f;
    //public float sensitivity = 100f;
    //public float clampAngle = 70f;

    //private float rotX;
    //private float rotY;

    //public Transform realCamera;
    //public Vector3 dirNormalized;
    //public Vector3 finalDir;
    //public float minDistance;
    //public float maxDistance;
    //public float finalDistance;
    //public float smooth = 10;

    private void Start()
    {
        //rotX = transform.localRotation.eulerAngles.x;
        //rotY = transform.localRotation.eulerAngles.y;

        //dirNormalized = realCamera.localPosition.normalized;
        //finalDistance = realCamera.localPosition.magnitude;
    }
    private void Update()
    {
        //rotX += -(Input.GetAxisRaw("Mouse Y")) * sensitivity * Time.deltaTime;
        //rotY += Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;

        //rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        //Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        //transform.rotation = rot;

    }
    private void A()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateX += mouseX * detailX;
        rotateX = rotateX > 180f ? rotateX - 360f : rotateX;
        rotateY += mouseY * detailY;
        rotateY = rotateY > 180f ? rotateY - 360f : rotateY;

        transform.localEulerAngles = new Vector3(-rotateY, rotateX, 0);
        transform.position = _target.position;
        _target.localEulerAngles = new Vector3(0, rotateX, 0);
    }
    private void LateUpdate()
    {
        //A();
        //transform.position = Vector3.MoveTowards(transform.position, objToFollow.position, followSpd * Time.deltaTime);
        //finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        //RaycastHit hit;

        //if (Physics.Linecast(transform.position, finalDir, out hit))
        //{
        //    finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        //}
        //else
        //{
        //    finalDistance = maxDistance;
        //}
        //realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smooth);
        //// ThirdCamera();
    }

}