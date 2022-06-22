using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{

    private Camera playerCamera;
    private float targetFov;
    private float fov;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();
        targetFov = playerCamera.fieldOfView;
        fov = targetFov;
    }
    private void Update()
    {
        float fovSpd = 4f;
        fov = Mathf.Lerp(fov, targetFov, Time.deltaTime * fovSpd);
        playerCamera.fieldOfView = fov;
    }
    public void SetCameraFov(float targetFov)
    {
        this.targetFov = targetFov;
    }

}
