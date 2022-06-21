using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMove : MonoBehaviour
{
    private const float NORMAL_FOV = 60f;
    private const float HOOKSHOT_FOV = 100f;
    public float mouseSensitivity = 1f;

    private CharacterController characterController;
    private float cameraVerticalAngle;
    private float characterVelocityY;
    private Vector3 characterVelocityMomentum;
    private Camera playerCamera;
    private Vector3 hookShotPosition;
    private State state;
    public Transform debughitpoint;
    public Transform hookShotTransform; 

    private float hookShotSize;
    private CameraFOV cameraFOV;
    private enum State
    {
        Normal,
        HookShotFlyingPlayer,
        HookShotTrown
    }
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        cameraFOV = playerCamera.GetComponent<CameraFOV>();
        Cursor.lockState = CursorLockMode.Locked; 
        state = State.Normal;
        hookShotTransform.gameObject.SetActive(false);
    }
    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                HandleCharacterLock();
                HandleCharacterMovement();
                HandleHookshotStart();
                break;
            case State.HookShotTrown:
                HandleHookShotThrow();
                HandleCharacterLock();
                HandleCharacterMovement();
                break;
            case State.HookShotFlyingPlayer:
                HandleCharacterLock();
                HandleHookshotMovement();
                break;
            default:
                break;
        }

    }
    private void HandleCharacterLock()
    {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");

        transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);

        cameraVerticalAngle -= lookY * mouseSensitivity;

        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);

    }
    private void HandleCharacterMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float moveSpeed = 20f;

        Vector3 characterVelocity = transform.right * moveX * moveSpeed + transform.forward * moveZ * moveSpeed;

        if (characterController.isGrounded)
        {
            characterVelocityY = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float jumpSpeed = 30f;
                characterVelocityY = jumpSpeed;
            }
        }
        float gravityDownForce = -60f;
        characterVelocityY += gravityDownForce * Time.deltaTime;

        characterVelocity.y = characterVelocityY;

        characterVelocity += characterVelocityMomentum;

        characterController.Move(characterVelocity * Time.deltaTime);

        if (characterVelocityMomentum.magnitude > 0f)
        {
            float momentumDrag = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if (characterVelocityMomentum.magnitude < 0f)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }
    }
    private void ResetGravityEffect()
    {
        characterVelocityY = 0f;
    }
    private void HandleHookshotStart()
    {
        if (TestInputDownHookshot())
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit))
            //if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit))
            {
                //hit ¼¶¾Å
                debughitpoint.position = raycastHit.point;
                hookShotPosition = raycastHit.point;
                hookShotSize = 0;
                hookShotTransform.gameObject.SetActive(true);
                hookShotTransform.localScale = Vector3.zero;

                state = State.HookShotTrown;
            }
        }
    }
    private void HandleHookShotThrow()
    {
        hookShotTransform.LookAt(hookShotPosition);
        float hookshotTrownSpeed = 500f;
        hookShotSize += hookshotTrownSpeed * Time.deltaTime;
        hookShotTransform.localScale = new Vector3(1, 1, hookShotSize);

        if (hookShotSize >= Vector3.Distance(transform.position, hookShotPosition))
        {
            state = State.HookShotFlyingPlayer;
            cameraFOV.SetCameraFov(HOOKSHOT_FOV); 
        }
    }
    private void HandleHookshotMovement()
    {
        hookShotTransform.LookAt(hookShotPosition);

        Vector3 hookShotDir = (hookShotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 40f;


        float hookShotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookShotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 5f;
        characterController.Move(hookShotDir * hookShotSpeed * hookshotSpeedMultiplier * Time.deltaTime);


        float reachedHookshotPositionDistance = 1f;
        if (Vector3.Distance(transform.position, hookShotPosition) < reachedHookshotPositionDistance)
        {
            StopHookShot();
        }
        if (TestInputDownHookshot())
        {
            StopHookShot();
        }
        if (TestInputJump())
        {
            float momentExtraSpeed = 7f;
            characterVelocityMomentum = hookShotDir * hookShotSpeed * momentExtraSpeed;
            float jumpSpeed = 40f;
            characterVelocityMomentum += Vector3.up * jumpSpeed;
            state = State.Normal;
            ResetGravityEffect();
            hookShotTransform.gameObject.SetActive(false);
        }
    }
    private void StopHookShot()
    {
        state = State.Normal;
        ResetGravityEffect();
        hookShotTransform.gameObject.SetActive(false);
        cameraFOV.SetCameraFov(NORMAL_FOV);
    }
    private bool TestInputDownHookshot()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    private bool TestInputJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
