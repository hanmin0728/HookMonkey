using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    //ĳ���� ���� �̵� �ӵ� (�ȱ�)
    public float walkMoveSpd = 2.0f;

    //ĳ���� ���� �̵� �ӵ� (�޸���)
    public float runMoveSpd = 3.5f;

    //ĳ���� ȸ�� �̵� �ӵ� 
    public float rotateMoveSpd = 100.0f;

    //ĳ���� ȸ�� �������� ���� ������ �ӵ�
    public float rotateBodySpd = 2.0f;

    //ĳ���� �̵� �ӵ� ���� ��
    public float moveChageSpd = 0.1f;

    //���� ĳ���� �̵� ���� �� 
    private Vector3 vecNowVelocity = Vector3.zero;

    //���� ĳ���� �̵� ���� ���� 
    private Vector3 vecMoveDirection = Vector3.zero;

    //CharacterController ĳ�� �غ�
    private CharacterController controllerCharacter = null;

    //ĳ���� CollisionFlags �ʱⰪ ����
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    //ĳ���� �߷°�
    public float gravity = 9.8f;

    //ĳ���� �߷� �ӵ� ��
    private float verticalSpd = 0f;

    //ĳ���� ���� ���� �÷���
    private bool stopMove = false;

    public GameObject knifeObj;
    public enum PlayerState { None, Idle, Walk, Hook, Attack }

    [Header("ĳ���ͻ���")]
    public PlayerState playerState = PlayerState.None;


    //[Header("��������")]
    ////������ ���� ������
    //public TrailRenderer AtkTrailRenderer = null;

    ////���⿡ �ִ� �ݶ��̴� ĳ��
    //public CapsuleCollider AtkCapsuleCollider = null;

    //[Header("��ų����")]
    //public AnimationClip skillAnimClip = null;
    //public GameObject skillEffect = null;


    private Animator anim;

    void Start()
    {
        //CharacterController ĳ��
        controllerCharacter = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();
        //�ʱ� �ִϸ��̼��� ���� Enum
        playerState = PlayerState.Idle;

    }

    void Update()
    {
        //ĳ���� �̵� 
        Move();

        //ĳ���� ���� ���� 
        vecDirectionChangeBody();


        //�÷��̾� ���� ���ǿ� ���߾� �ִϸ��̼� ��� 
        ckAnimationState();

        AnimationSet();
        //�߷� ����
        setGravity();
    
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
        //���� ī�޶� �ٶ󺸴� ������ ����� � �����ΰ�.
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //forward.z, forward.x
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //Ű�Է� 
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //�ɸ��Ͱ� �̵��ϰ��� �ϴ� ���� 
        Vector3 targetDirection = horizontal * right + vertical * forward;

        //���� �̵��ϴ� ���⿡�� ���ϴ� �������� ȸ�� 

        vecMoveDirection = Vector3.RotateTowards(vecMoveDirection, targetDirection, rotateMoveSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        vecMoveDirection = vecMoveDirection.normalized;
        //ĳ���� �̵� �ӵ�
        float spd = walkMoveSpd;

        //���࿡ playerState�� Walk�̸� 

        if (playerState == PlayerState.Walk)
        {
            spd = walkMoveSpd;
        }

        //�߷� ����
        Vector3 vecGravity = new Vector3(0f, verticalSpd, 0f);


        // ������ �̵� ��
        Vector3 moveAmount = (vecMoveDirection * spd * Time.deltaTime) + vecGravity;

        collisionFlagsCharacter = controllerCharacter.Move(moveAmount);
    }

    /// <summary>
    /// ���� �� �ɸ��� �̵� �ӵ� �������� ��  
    /// </summary>
    /// <returns>float</returns>
    float getNowVelocityVal()
    {
        //���� ĳ���Ͱ� ���� �ִٸ� 
        if (controllerCharacter.velocity == Vector3.zero)
        {
            //��ȯ �ӵ� ���� 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {
            //��ȯ �ӵ� ���� ����
            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }
        //�Ÿ� ũ��
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
            //ĳ���� ���� �ӵ�
            float _getVelocitySpd = getNowVelocityVal();
            GUILayout.Label("����ӵ� : " + _getVelocitySpd.ToString(), labelStyle);

            //���� ĳ���� ���� + ũ��
            GUILayout.Label("���纤�� : " + controllerCharacter.velocity.ToString(), labelStyle);

            //����  ����� ũ�� �ӵ�
            GUILayout.Label("������� ũ�� �ӵ� : " + vecNowVelocity.magnitude.ToString(), labelStyle);

        }
    }

    /// <summary>
    /// ĳ���� ���� ���� ���� �Լ�
    /// </summary>
    void vecDirectionChangeBody()
    {
        //ĳ���� �̵� ��
        if (getNowVelocityVal() > 0.0f)
        {
            //�� ����  �ٶ���� �ϴ� ���� ���?
            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;

            //�� ĳ���� ���� ���� 
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
                    playerState = PlayerState.Walk;
                }
                break;
            default:
                break;
        }
    }

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
    IEnumerator Atk()
    {
        knifeObj.tag = "PlayerAtk";
        yield return new WaitForSeconds(3f);
        knifeObj.tag = "Untagged";
    }


    ////��ų �ִϸ��̼� ����� ������ �� �̺�Ʈ 
    //void OnSkillAnimFinished()
    //{
    //    //���� ĳ���� ��ġ ����
    //    Vector3 pos = transform.position;

    //    //ĳ���� �� ���� 2.0���� ������ �Ÿ� 
    //    pos += transform.forward * 2f;

    //    //�� ��ġ�� ��ų ����Ʈ�� ���δ�. 
    //    Instantiate(skillEffect, pos, Quaternion.identity);

    //    //�������� ��� ���·� �д�. 
    //    playerState = PlayerState.Idle;
    //}



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
