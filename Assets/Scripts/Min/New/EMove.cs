using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EMove : MonoBehaviour, IHittable
{
    public bool isAttack;

    private Rigidbody _rigidbody;
    [SerializeField] private int _hp = 5;
    public LayerMask layerMask;

    [SerializeField]
    private GameObject damageEffect;
    bool isDamaged = false;
    [SerializeField] private UnityEvent IHittable;
    UnityEvent IHittable.OnDamage { get => IHittable; set { } }

    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer = null; // 몸통 스킨 메시
    private EnemyState _enemyState;
    NavMeshAgent _nav;
    private Transform target;

    private float detectedRange = 15f;
    private enum EnemyState
    {
        Idle, // 일정 감지 전까지 가만있기
        Walk, // 플레이어 따라가기
        Attack, // 공격
        Damage, //데미지 
        Die // 죽는거
    }
    Animator _animator;
    [SerializeField] private bool isChase;
    Vector3 posTarget = Vector3.zero;

    private void Awake()
    {
        target = GameManager.Instance.Player;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _enemyState = EnemyState.Idle;
        _nav = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        CheckState();
        CheckPlayer();
        UpGround();
        if (isChase)
        {
            _nav.SetDestination(target.position); //도착할 목표 위치 지정 함
        }
        EnemyAttack();
    }
    void UpGround()
    {
        posTarget = new Vector3(transform.position.x + Random.Range(-10f, 10f),
                    transform.position.y + 1000f,
                    transform.position.z + Random.Range(-10f, 10f)
                    );

        Ray ray = new Ray(posTarget, Vector3.down);

        RaycastHit infoRayCast = new RaycastHit();

        if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
        {
            posTarget.y = infoRayCast.point.y;
        }
    }
    void FreezeVelocity()
    {
        if (isChase)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

        }
    }
    private void FixedUpdate()
    {
        FreezeVelocity();
    }
    void CheckPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(0, 2f, 0), detectedRange, layerMask);

        if (hitColliders.Length > 0)
        {
            isChase = true;
            _enemyState = EnemyState.Walk;
            _animator.SetBool("isWalk", true);
        }

       
    }

    void CheckState()
    {
        switch (_enemyState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Damage:
                break;
            case EnemyState.Die:
                break;
            default:
                break;
        }
    }
    public void EnemyAttack()
    {
        float atkRange = 6f;
        if (Vector3.Distance(transform.position, GameManager.Instance.Player.position) < atkRange)
        {
            print(Vector3.Distance(transform.position, GameManager.Instance.Player.position));
            if (isAttack)
            {
                return;
            }
            isAttack = true;
            StartCoroutine(Attack());
        }
    }
    IEnumerator Attack()
    {
        _animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(7f);
        isAttack = false;
    }
    public void Damage()
    {
        StartCoroutine(DamageCoroutine());
    }
    IEnumerator DamageCoroutine()
    {
        if (isDamaged)
            yield break;
        if (_hp > 0)
        {
            Debug.Log("Damage");
            isDamaged = true;
            _enemyState = EnemyState.Damage;
            Instantiate(damageEffect, transform.position, Quaternion.identity);
            for (int i = 0; i < 4; i++)
            {
                skinnedMeshRenderer.material.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                skinnedMeshRenderer.material.color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(2f);
            _enemyState = EnemyState.Idle;
            isDamaged = false;
        }
        else
        {
            Die();
        }
    }
    public void Die()
    {
        _enemyState = EnemyState.Die;
    }
}
