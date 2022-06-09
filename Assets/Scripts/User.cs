using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    //캐릭터 직선 이동 속도 (걷기)
    public float walkMoveSpd = 2.0f;

    //캐릭터 직선 이동 속도 (달리기)
    public float runMoveSpd = 3.5f;

    //캐릭터 회전 이동 속도 
    public float rotateMoveSpd = 100.0f;

    //캐릭터 회전 방향으로 몸을 돌리는 속도
    public float rotateBodySpd = 2.0f;

    //캐릭터 이동 속도 증가 값
    public float moveChageSpd = 0.1f;

    //현재 캐릭터 이동 백터 값 
    private Vector3 vecNowVelocity = Vector3.zero;

    //현재 캐릭터 이동 방향 벡터 
    private Vector3 vecMoveDirection = Vector3.zero;

    //CharacterController 캐싱 준비
    private CharacterController controllerCharacter = null;

    //캐릭터 CollisionFlags 초기값 설정
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    //캐릭터 중력값
    public float gravity = 9.8f;

    //캐릭터 중력 속도 값
    private float verticalSpd = 0f;

    //캐릭터 멈춤 변수 플래그
    private bool stopMove = false;

    public GameObject knifeObj;
    public enum PlayerState { None, Idle, Walk, Hook, Attack }

    [Header("캐릭터상태")]
    public PlayerState playerState = PlayerState.None;


    //[Header("전투관련")]
    ////공격할 때만 켜지게
    //public TrailRenderer AtkTrailRenderer = null;

    ////무기에 있는 콜라이더 캐싱
    //public CapsuleCollider AtkCapsuleCollider = null;

    //[Header("스킬관련")]
    //public AnimationClip skillAnimClip = null;
    //public GameObject skillEffect = null;


    private Animator anim;

    void Start()
    {
        //CharacterController 캐싱
        controllerCharacter = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();
        //초기 애니메이션을 설정 Enum
        playerState = PlayerState.Idle;

    }

    void Update()
    {
        //캐릭터 이동 
        Move();

        //캐릭터 방향 변경 
        vecDirectionChangeBody();


        //플레이어 상태 조건에 맞추어 애니메이션 재생 
        ckAnimationState();

        AnimationSet();
        //중력 적용
        setGravity();
    
    }

    /// <summary>
    /// 이동함수 입니다 캐릭터
    /// </summary>
    void Move()
    {
        if (stopMove == true)
        {
            return;
        }

        Transform CameraTransform = Camera.main.transform;
        //메인 카메라가 바라보는 방향이 월드상에 어떤 방향인가.
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //forward.z, forward.x
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //키입력 
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //케릭터가 이동하고자 하는 방향 
        Vector3 targetDirection = horizontal * right + vertical * forward;

        //현재 이동하는 방향에서 원하는 방향으로 회전 

        vecMoveDirection = Vector3.RotateTowards(vecMoveDirection, targetDirection, rotateMoveSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        vecMoveDirection = vecMoveDirection.normalized;
        //캐릭터 이동 속도
        float spd = walkMoveSpd;

        //만약에 playerState가 Walk이면 

        if (playerState == PlayerState.Walk)
        {
            spd = walkMoveSpd;
        }

        //중력 백터
        Vector3 vecGravity = new Vector3(0f, verticalSpd, 0f);


        // 프레임 이동 양
        Vector3 moveAmount = (vecMoveDirection * spd * Time.deltaTime) + vecGravity;

        collisionFlagsCharacter = controllerCharacter.Move(moveAmount);
    }

    /// <summary>
    /// 현재 내 케릭터 이동 속도 가져오는 함  
    /// </summary>
    /// <returns>float</returns>
    float getNowVelocityVal()
    {
        //현재 캐릭터가 멈춰 있다면 
        if (controllerCharacter.velocity == Vector3.zero)
        {
            //반환 속도 값은 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {
            //반환 속도 값은 현재
            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }
        //거리 크기
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
            //캐릭터 현재 속도
            float _getVelocitySpd = getNowVelocityVal();
            GUILayout.Label("현재속도 : " + _getVelocitySpd.ToString(), labelStyle);

            //현재 캐릭터 방향 + 크기
            GUILayout.Label("현재벡터 : " + controllerCharacter.velocity.ToString(), labelStyle);

            //현재  재백터 크기 속도
            GUILayout.Label("현재백터 크기 속도 : " + vecNowVelocity.magnitude.ToString(), labelStyle);

        }
    }

    /// <summary>
    /// 캐릭터 몸통 벡터 방향 함수
    /// </summary>
    void vecDirectionChangeBody()
    {
        //캐릭터 이동 시
        if (getNowVelocityVal() > 0.0f)
        {
            //내 몸통  바라봐야 하는 곳은 어디?
            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;

            //내 캐릭터 전면 설정 
            transform.forward = Vector3.Lerp(transform.forward, newForward, rotateBodySpd * Time.deltaTime);

        }
    }

    /// <summary>
    ///  현재 상태를 체크해주는 함수
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


    ////스킬 애니메이션 재생이 끝났을 때 이벤트 
    //void OnSkillAnimFinished()
    //{
    //    //현재 캐릭터 위치 저장
    //    Vector3 pos = transform.position;

    //    //캐릭터 앞 방향 2.0정도 떨어진 거리 
    //    pos += transform.forward * 2f;

    //    //그 위치에 스킬 이펙트를 붙인다. 
    //    Instantiate(skillEffect, pos, Quaternion.identity);

    //    //끝났으면 대기 상태로 둔다. 
    //    playerState = PlayerState.Idle;
    //}



    /// <summary>
    ///  캐릭터 중력 설정
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
