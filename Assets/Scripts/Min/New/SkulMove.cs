using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SkulMove : MonoBehaviour
{
    // 상태
    public enum States
    {
        NONE,
        IDLE,
        MOVE,
        WAIT,
        GOTARGET,
        ATK,
        DAMAGE,
        DIE
    }

    [Header("기본속성")]
    public States _state = States.NONE; 
    public float spdMove = 2f;
    [SerializeField]
    private GameObject damageObj = null; // 데미지를 받앗을 때 나오는 오브젝트

    public GameObject targetCharacter = null; // 타겟
    public Transform targetTransform = null;
    public Vector3 posTarget = Vector3.zero;
    private Animation _animation = null;
    private Transform _transform = null;

    [Header("애니메이션 클립")]
    public AnimationClip IdleAnimationClip = null;
    public AnimationClip MoveAnimaitonClip = null;
    public AnimationClip AttackAnimaitonClip = null;
    public AnimationClip DamageAnimationClip = null;
    public AnimationClip DieAnimationClip = null;

    [Header("전투속성")]
    public int hp = 5; // 체력
    public float AtkRange = 1.5f; // 공격 사거리
    public GameObject effectDamage = null; // 피격 시 나올 이펙트
    public GameObject effectDie = null; // 죽었을 시 나올 이펙트
    public float moveRadius = 10f; // 이동 반경
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer = null; // 몸통 스킨 메시

    [SerializeField]
    private BoxCollider _atkCollider = null; // 공격 콜라이더
    [SerializeField]
    private Collider _col = null; // 스컬의 콜라이더
    [SerializeField]
    private float attackDistance = 0.3f; // 실제적 공격 거리

    private Action<int> OnDieEvent = null; // 죽었을 때 발행할 이벤트

    private Rigidbody _rigid = null; // 리지드바디 캐싱 준비

    private bool _isAttack = false; // 현재 공격중인가?

    [SerializeField]
    private GameObject _light = null;

    private int _damage = 1;
    public int damage
    {
        get => _damage;
        set
        {
            _damage = value;
        }
    }

    #region 애니메이션 이벤트
    private void OnAtkAnimationFinished()
    {
        Debug.Log("Atk animation Finished");
        _isAttack = false;
        _atkCollider.enabled = false;
    }
    private void OnDamageAnimationFinished()
    {
        skinnedMeshRenderer.material.color = Color.white;
        Debug.Log("Damage animation Finished");
        _isAttack = false;
    }
    private void OnDieAnimationFinished()
    {
        Debug.Log("Die animation Finished");

        Instantiate(effectDie, transform.position + new Vector3(0f, 0.5f, 0f), transform.rotation);
    }
    void OnAnimationEvent(AnimationClip clip, string funcNmae)
    {
        //이벤트 만들어줌
        AnimationEvent newAnimationEvent = new AnimationEvent();
        //이벤트 함수 연결
        newAnimationEvent.functionName = funcNmae;
        //끝나기 직전에 호출
        newAnimationEvent.time = clip.length - 0.2f;
        //이벤트 넣어줌
        clip.AddEvent(newAnimationEvent);
    }
    #endregion
    private void Start()
    {
        // 초기화
        _state = States.IDLE;
        // 캐싱
        _transform = GetComponent<Transform>();
        _animation = GetComponent<Animation>();
        _rigid = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();

        // 애니메이션 초기화
        _animation[IdleAnimationClip.name].wrapMode = WrapMode.Loop;
        _animation[MoveAnimaitonClip.name].wrapMode = WrapMode.Loop;
        _animation[DieAnimationClip.name].wrapMode = WrapMode.Once;
        _animation[DieAnimationClip.name].layer = 5;
        _animation[DamageAnimationClip.name].wrapMode = WrapMode.Once;
        _animation[DamageAnimationClip.name].layer = 5;
        _animation[AttackAnimaitonClip.name].wrapMode = WrapMode.Once;
        _animation[DamageAnimationClip.name].layer = 10;
        _animation[AttackAnimaitonClip.name].speed = 1.5f;

        OnAnimationEvent(AttackAnimaitonClip, "OnAtkAnimationFinished");
        OnAnimationEvent(DamageAnimationClip, "OnDamageAnimationFinished");
        OnAnimationEvent(DieAnimationClip, "OnDieAnimationFinished");

        spdMove = Random.Range(spdMove - 1f, spdMove + 1f); // 스피드를 랜덤으로

    }
    private void Update()
    {
        CheckState();
        AnimationCtrl();
    }

    private void StartInit()
    {
        Debug.Log("해골 이닛 시작");
    }

    /// <summary>
    /// 해골 상태에 따라 동작을 제어하는 함수
    /// </summary>
    void CheckState()
    {
        switch (_state)
        {
            case States.IDLE:
                _isAttack = false;
                SetIdle();
                break;
            case States.GOTARGET:
            case States.MOVE:
                if (_isAttack == false)
                    SetMove();
                break;
            case States.WAIT:
                break;
            case States.ATK:
                SetAttack();
                break;
            case States.DAMAGE:
                break;
            case States.DIE:
                break;
            default:
                break;

        }
    }
    /// <summary>
    /// 임의의 지역 이동 후 대기모드
    /// </summary>
    /// <returns></returns>
    IEnumerator SetWait()
    {
        //해골 상태를 대기상태로 변경
        Debug.Log("대기");
        _state = States.WAIT;
        //대기하는 시간
        float timeWait = Random.Range(1f, 3f);
        //대기 시간을 넣어줘야함
        yield return new WaitForSeconds(timeWait);
        _state = States.IDLE;
    }

    /// <summary>
    /// 공격 셋팅
    /// </summary>
    void SetAttack()
    {
        Vector3 temp = targetTransform.position - transform.position;
        if (temp.magnitude > AtkRange * AtkRange + attackDistance)
        {
            _state = States.GOTARGET;
        }

        float distance = Vector3.Distance(targetTransform.position, _transform.position);

        if (distance > AtkRange + attackDistance)
        {
            _state = States.GOTARGET;
        }
        //to be continue
    }

    /// <summary>
    /// 목표물 설정
    /// </summary>
    /// <param name="target"></param>
    private void OnCkTarget(GameObject target)
    {
        targetCharacter = target;

        targetTransform = targetCharacter.transform;

        _state = States.GOTARGET;
    }

    /// <summary>
    /// 상태가 무브일 때 동작
    /// </summary>
    void SetMove()
    {
        //출발점 도착점 두 벡터 차이
        Vector3 distance = Vector3.zero;
        //방향
        Vector3 posLookAt = Vector3.zero;
        switch (_state)
        {
            case States.MOVE:
                //타겟 없을 때
                if (posTarget != Vector3.zero)
                {
                    //차이 구하기
                    distance = posTarget - _transform.position;
                    //만약 distance의 길이가 range보다 작으면
                    if (distance.magnitude < AtkRange)
                    {
                        StartCoroutine(SetWait());
                        return;
                    }
                    //방향 구하기
                    posLookAt = new Vector3(posTarget.x, _transform.position.y, posTarget.z);
                }
                break;
            case States.GOTARGET:
                //타겟 있을 때
                if (targetCharacter != null)
                {
                    //타겟과 차이
                    distance = targetCharacter.transform.position - _transform.position;

                    if (distance.magnitude < AtkRange)
                    {
                        _state = States.ATK;
                        return;
                    }

                    posLookAt = new Vector3(targetCharacter.transform.position.x,
                        _transform.position.y,
                        targetCharacter.transform.position.z
                        );
                }
                break;
            default:
                break;
        }

        Vector3 direction = distance.normalized;

        direction = new Vector3(direction.x, 0f, direction.z);

        Vector3 amount = direction * spdMove * Time.deltaTime;

        _transform.Translate(amount, Space.World);

        _transform.LookAt(posLookAt);
    }

    /// <summary>
    /// 상태가 대기일 때 동작
    /// </summary>
    void SetIdle()
    {
        if (targetCharacter == null)
        {
            posTarget = new Vector3(_transform.position.x + Random.Range(-moveRadius, moveRadius),
                _transform.position.y + 1000f,
                _transform.position.z + Random.Range(-moveRadius, moveRadius)
                );
            //레이캐스트 시작점 목표 방향
            Ray ray = new Ray(posTarget, Vector3.down);
            //충돌체 정보 변수
            RaycastHit infoRaycast = new RaycastHit();
            //만약 충돌체가 있냐면
            if (Physics.Raycast(ray, out infoRaycast, Mathf.Infinity))
            {
                //임의의 목표 벡터에 높이값을 추가
                posTarget.y = infoRaycast.point.y;
            }
            // 상태를 무브로
            _state = States.MOVE;
        }
        else
        {

            // 상태를 고타겟으로
            _state = States.GOTARGET;
        }
    }
    /// <summary>
    /// 애니메이션 재생 함수
    /// </summary>
    void AnimationCtrl()
    {
        // 상태에 따라 애니메이션 적용
        switch (_state)
        {
            case States.WAIT:
            case States.IDLE:
                _animation.CrossFade(IdleAnimationClip.name);
                break;
            case States.MOVE:
            case States.GOTARGET:
                if (_isAttack == false)
                    _animation.CrossFade(MoveAnimaitonClip.name);
                break;
            case States.ATK:
                _animation.CrossFade(AttackAnimaitonClip.name);
                _isAttack = true;
                _atkCollider.enabled = true;
                break;
            case States.DIE:
                _animation.CrossFade(DieAnimationClip.name);
                break;
            case States.DAMAGE:
                _animation.CrossFade(IdleAnimationClip.name);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 피격 함수
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        hp -= damage;

        if (hp > 0)
        {
            Instantiate(effectDie, transform.position + new Vector3(0f, 0.5f, 0f), transform.rotation); // 피격 이펙트 생성

            _animation.CrossFade(DamageAnimationClip.name);

            EffectDamageTween(); // 피격 이벤트 

            _rigid.AddForce(transform.forward * -1 * 0f, ForceMode.Impulse); // 뒤 방향으로 넉백
            Invoke("ResetVelocity", 0.5f); // 벨로시티 초기화하기
        }
        else
        {
            if (_state == States.DIE)
                return;
            _state = States.DIE;

            _col.enabled = false; // 스컬 콜라이더 없애기
            _atkCollider.enabled = false; // 공격 콜라이더 없애기

            OnDieEvent?.Invoke(1); // 경험치 증가시키기

            Destroy(gameObject, DieAnimationClip.length - 0.15f); // DIE 애니메이션 실행 후 디스트로이

            GameObject obj = Instantiate(damageObj, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.Euler(new Vector3(90f, 0f, 0f))); // 죽었을 때 나올 오브젝트 생성
            obj.transform.SetParent(null);
            obj.SetActive(true);
            obj.transform.DOMoveY(transform.position.y + 1f, 1f);
        }
    }

    /// <summary>
    /// 리지드바디 속도 초기화 함수
    /// </summary>
    public void ResetVelocity() => _rigid.velocity = Vector3.zero;

    /// <summary>
    /// 맞았을 때 실행될 함수
    /// </summary>
    private void EffectDamageTween()
    {
        StartCoroutine(Damaged()); // 색깔을 바꾸는 함수
    }

    /// <summary>
    /// 맞았을 때 실행될 함수 2
    /// </summary>
    /// <returns></returns>
    private IEnumerator Damaged()
    {
        for (int i = 0; i < 4; i++)
        {
            skinnedMeshRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            skinnedMeshRenderer.material.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private int _attackDamage = 1;
 
}
