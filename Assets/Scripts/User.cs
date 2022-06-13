using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    public float walkMoveSpd = 2.0f;

    public float rotateMoveSpd = 100.0f;

    public float rotateBodySpd = 2.0f;

    public float moveChageSpd = 0.1f;

    private Vector3 vecNowVelocity = Vector3.zero;

    private Vector3 vecMoveDirection = Vector3.zero;
    private CharacterController controllerCharacter = null;

    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    public float gravity = 9.8f;
    private float verticalSpd = 0f;

    private bool stopMove = false;

    public GameObject knifeObj;
    public enum PlayerState { None, Idle, Walk, Hook, Attack }

    [Header("ĳ���ͻ���")]
    public PlayerState playerState = PlayerState.None;

    public Text text;
    private Animator anim;

    void Start()
    {
        controllerCharacter = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();
        playerState = PlayerState.Idle;

    }

    void Update()
    {
        Move();

        vecDirectionChangeBody();
        ShowPlayerState();
        ckAnimationState();
        AnimationSet();
        setGravity();
    
    }
    void ShowPlayerState()
    {
        text.text = "���纤�� : " + controllerCharacter.velocity.ToString();
    }

    /// <summary>
    /// �̵��Լ� �Դϴ� ĳ����
    /// </summary>
    void Move()
    {
        if (stopMove == true)
        {
            return;
        }

        Transform CameraTransform = Camera.main.transform;
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 targetDirection = horizontal * right + vertical * forward;


        vecMoveDirection = Vector3.RotateTowards(vecMoveDirection, targetDirection, rotateMoveSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        vecMoveDirection = vecMoveDirection.normalized;
        float spd = walkMoveSpd;


        if (playerState == PlayerState.Walk)
        {
            spd = walkMoveSpd;
        }

        Vector3 vecGravity = new Vector3(0f, verticalSpd, 0f);

        Vector3 moveAmount = (vecMoveDirection * spd * Time.deltaTime) + vecGravity;

        collisionFlagsCharacter = controllerCharacter.Move(moveAmount);
    }

    /// <summary>
    /// ���� �� �ɸ��� �̵� �ӵ� �������� �Լ�
    /// </summary>
    float getNowVelocityVal()
    {
        if (controllerCharacter.velocity == Vector3.zero)
        {
            vecNowVelocity = Vector3.zero;
        }
        else
        {
            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }
        return vecNowVelocity.magnitude;
    }

    /// <summary>
	/// GUI SKin
	/// </summary>
    private void OnGUI()
    {
        if (controllerCharacter != null && controllerCharacter.velocity != Vector3.zero)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 50;
            labelStyle.normal.textColor = Color.white;
            float _getVelocitySpd = getNowVelocityVal();
            GUILayout.Label("����ӵ� : " + _getVelocitySpd.ToString(), labelStyle);
            GUILayout.Label("������� ũ�� �ӵ� : " + vecNowVelocity.magnitude.ToString(), labelStyle);

        }
    }

    /// <summary>
    /// ĳ���� ���� ���� ���� �Լ�
    /// </summary>
    void vecDirectionChangeBody()
    {
        if (getNowVelocityVal() > 0.0f)
        {
            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;

            transform.forward = Vector3.Lerp(transform.forward, newForward, rotateBodySpd * Time.deltaTime);

        }
    }

    /// <summary>
    ///  ���� ���¸� üũ���ִ� �Լ�
    /// </summary>
    void ckAnimationState()
    {
        float nowSpd = getNowVelocityVal();

        switch (playerState)
        {
            case PlayerState.Idle:
                if (nowSpd > 0.0f)
                {
                    playerState = PlayerState.Walk;
                }
                break;
            case PlayerState.Walk:
                if (nowSpd < 0.01f)
                {
                    playerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Attack:
                if (nowSpd < 0.01f)
                {
                    playerState = PlayerState.Idle;
                }
                else if (nowSpd > 0.0f)
                {
                    playerState = PlayerState.Walk; //������ ������ ��� ���̵�
                }
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// ��ī�� �ִϸ��̼� �Լ�
    /// </summary>
    void AnimationSet()
    {
        anim.SetInteger("State", (int)playerState);

        if (Input.GetMouseButton(0))
        {
            playerState = PlayerState.Attack;
            StartCoroutine(Atk());
            anim.SetTrigger("Attack");  

        }
    }
    /// <summary>
    /// �����ҋ��� �±׹ٲ��ִ� �Լ�
    /// </summary>
    IEnumerator Atk()
    {
        knifeObj.tag = "PlayerAtk";
        yield return new WaitForSeconds(3f);  // �ٳ��� �±� �ٲ۰�
        knifeObj.tag = "Untagged";
    }


   
    /// <summary>
    ///  ĳ���� �߷� ����
    /// </summary>
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

}
