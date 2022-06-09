using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float speed = 2f;
    public float bodyRotateSpeed = 100f;
    //캐릭터 CollisionFlags 초기값 설정
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    //캐릭터 중력값
    public float gravity = 9.8f;

    //캐릭터 중력 속도 값
    private float verticalSpd = 0f;
    public enum PlayerState { None, Idle, Walk, Hook, Attack }

    [Header("캐릭터상태")]
    public PlayerState playerState = PlayerState.None;

    public GameObject effectDamage = null;

    Vector3 CurrentVelocitySpd = Vector3.zero;
    float VelocityChangeSpd = 0.1f;

    private CharacterController characterController = null;
    private Animator animator = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        setGravity();
        AnimationUpdate();

        if (Input.GetMouseButtonDown(0))
        {
            AnimationStart();
        }
    }

    private void AnimationStart()
    {
        animator.SetInteger("State", (int)playerState);

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            playerState = PlayerState.Attack;
        }
    }


    private void AnimationUpdate()
    {
        speed = GetVelocitySpd();
        switch (playerState)
        {
            case PlayerState.Idle:
                if (speed > 0.0f)
                {
                    playerState = PlayerState.Walk;

                }
                break;
            case PlayerState.Walk:
                if (speed < 0.01f)
                {
                    playerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Attack:
                if (speed < 0.01f)
                {
                    playerState = PlayerState.Idle;
                }
                else if (speed > 0.0f)
                {
                    playerState = PlayerState.Walk;
                }
                break;
            default:
                break;
        }
    }



    private void BodyRotate(Vector3 dir)
    {
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), bodyRotateSpeed * Time.deltaTime);
    }

    private void OnGUI()
    {
        if (characterController != null)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 50;
            labelStyle.normal.textColor = Color.white;

            GUILayout.Label("MOVE : " + GetVelocitySpd().ToString(), labelStyle);
            GUILayout.Label("플레이어 상태 : " + playerState.ToString(), labelStyle);
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0f;
        Vector3 right = new Vector3(forward.z, 0f, -forward.x);

        Vector3 amount = h * right + v * forward;

        characterController.Move(amount * speed * Time.deltaTime);

        BodyRotate(amount);
    }

    float GetVelocitySpd()
    {
        if (characterController.velocity == Vector3.zero)
        {
            CurrentVelocitySpd = Vector3.zero;
        }
        else
        {
            Vector3 retVelocity = characterController.velocity;
            retVelocity.y = 0;
            CurrentVelocitySpd = Vector3.Lerp(
                CurrentVelocitySpd, retVelocity, VelocityChangeSpd * Time.fixedDeltaTime);
        }
        return CurrentVelocitySpd.magnitude;
    }
    void setGravity()
    {
        if ((collisionFlagsCharacter & CollisionFlags.CollidedBelow) != 0)
        {
            verticalSpd = 0f;
        }
        else
        {
            verticalSpd -= gravity * Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("MonsterAtk") == true)
        //{
        //    Instantiate(effectDamage, other.transform.position, Quaternion.identity);
        //}
    }
}