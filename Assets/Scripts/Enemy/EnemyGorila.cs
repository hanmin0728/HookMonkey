using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyGorila : MonoBehaviour
{
    [SerializeField] private int _hp = 5; 

    Rigidbody _rigidbody;
    CapsuleCollider _capsuleCollider;
    Material _mat;
    public bool isChase = false;
    public Transform target;
    NavMeshAgent _nav;
    Animator _animator;
    public bool isDamage;
    public GameObject dieEffect;
    public GameObject damageEffect;
    public GameObject item;
    CharacterMove cm;


    private void Awake()
    {
        cm = FindObjectOfType<CharacterMove>();
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        _nav = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        Invoke("ChaseStart", 2f);
    }
    void ChaseStart()
    {
        isChase = true;
        _animator.SetBool("isWalk", true);
    }
    private void Update()
    {
        if (isChase)
        {
            _nav.SetDestination(target.position); //도착할 목표 위치 지정 함
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAtk") == true)
        {
            Instantiate(damageEffect, other.transform.position, Quaternion.identity);
            StartCoroutine(Damage());
            if (_hp < 0)
            {
                isChase = false;
                _animator.SetTrigger("Die");
                StartCoroutine(Die());
                _nav.enabled = false;
            }
        }
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(dieEffect, transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
        Instantiate(item, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    IEnumerator Damage()
    {
        transform.DOJump(Vector3.up, 0.2f, 1, 2f);
        Vector3 amount;
        amount = transform.position;
        Vector3 randomUnit = Random.insideUnitSphere;
        randomUnit.y = 0f;
        transform.DOMove(amount - randomUnit * 10f, 2f);
        _hp -= cm.damage;
        yield return new WaitForSeconds(2f);
    }
}
