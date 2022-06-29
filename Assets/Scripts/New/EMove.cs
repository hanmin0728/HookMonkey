using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Events;
using DG.Tweening;
public class EMove : MonoBehaviour, IHittable
{
    public bool isAttack;

    private Rigidbody _rigidbody;
    [SerializeField] private int _hp = 5;
    public int _maxHp;

    public LayerMask layerMask;

    [SerializeField]
    private GameObject damageEffect; 
    [SerializeField]
    private GameObject dieeEffect;
    bool isDamaged = false;
    [SerializeField] private UnityEvent IHittable;
    UnityEvent IHittable.OnDamage { get => IHittable; set { } }

    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer = null; // ���� ��Ų �޽�
    private EnemyState _enemyState;
    NavMeshAgent _nav;
    private Transform target;

    private float detectedRange = 15f;
    public GameObject ItemPrefab;
    private enum EnemyState
    {
        Idle, // ���� ���� ������ �����ֱ�
        Walk, // �÷��̾� ���󰡱�
        Attack, // ����
        Damage, //������ 
        Die // �״°�
    }
    Animator _animator;
    [SerializeField] private bool isChase;
    public bool isDie = false;
    Vector3 posTarget = Vector3.zero;

    public Collider leftHandColider;
    public Collider rightHandColider;
    MonkeyMove monkey;

    public Transform hpTransform;
    Camera cam;
    public CameraShake cameraShake;

    public Image enemyHpImage;

    #region �������ǵ�� ����
    public float stopTime;
    public bool stopping;
    public float slowTime;
    #endregion
    public CapsuleCollider cap;
    private void Awake()
    {
        target = GameManager.Instance.Player;
        _animator = GetComponent<Animator>();
        monkey = FindObjectOfType<MonkeyMove>();
        _rigidbody = GetComponent<Rigidbody>();
        _enemyState = EnemyState.Idle;
        _nav = GetComponent<NavMeshAgent>();
        cam = GameObject.Find("CameraHolder").GetComponentInChildren<Camera>();
    }
    private void Start()
    {
        _maxHp = _hp;   
    }
    private void Update()
    {
        if (isDie)
            return;

        Quaternion hp = Quaternion.LookRotation(hpTransform.position - cam.transform.position);
        Vector3 hp_angle = Quaternion.RotateTowards(hpTransform.rotation, hp, 200).eulerAngles;
        hpTransform.rotation = Quaternion.Euler(0, hp_angle.y, 0);

        CheckState();
        CheckPlayer();
        UpGround();
        if (isChase)
        {
            _nav.SetDestination(target.position); //������ ��ǥ ��ġ ���� ��
        }
        EnemyAttack();
        if (_hp <= 0)
        {
            if (isDie)
            {
                return;
            }
            Die();

            //Die();
        }
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
                // 

                break;
            case EnemyState.Walk:
                //�ȴ� �ڵ�
                break;
            case EnemyState.Attack:
                // �����ϴ� �ڵ�
                break;
            case EnemyState.Damage:
                // 
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
        yield return new WaitForSeconds(1.5f);
        rightHandColider.enabled = true;
        leftHandColider.enabled = true;
        yield return new WaitForSeconds(3f);
        rightHandColider.enabled = false;
        leftHandColider.enabled = false;
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
            //Debug.Log("Damage");
            isDamaged = true;
            _enemyState = EnemyState.Damage;
            //monkey.durability--;
            _hp -= monkey.damage;
            Debug.Log(monkey.durability);
            monkey.durability -= 1;
            monkey.nagodoImage.fillAmount -= 1f / monkey.Maxdurability; // ��������������ƽ�����������
            SoundManager.Instance.PlaySE("�� ������");
            enemyHpImage.fillAmount -= 1f / _maxHp;
            //monkey.nagodoImage.fillAmount -= 1f / _maxHp;
            // monkey.nagodoBar.value -= 1;
            //  monkey.nagodoBar.value = Mathf.Lerp(monkey.nagodoBar.value, monkey.durability, Time.deltaTime * 10);
            Instantiate(damageEffect, transform.position, Quaternion.identity);
            KnockBack();
            yield return new WaitForSeconds(2f);
            _enemyState = EnemyState.Idle;
            isDamaged = false;
        }
      
    }
    public void KnockBack()
    {

        transform.DOJump(Vector3.up, 0.2f, 1, 2f);
        Vector3 amount = transform.position;
        Vector3 randomUnit = Random.insideUnitSphere;
        randomUnit.y = 0f;

        transform.DOMove(amount - randomUnit * 10f, 2f);
        if (_hp <= 1)
        {
            return;
        }

        TimeStop();
    }
    public void Die()
    {
        _enemyState = EnemyState.Die;
        rightHandColider.enabled = false;
        leftHandColider.enabled = false;
        cap.direction = 2;
        _animator.SetTrigger("Die");
        SoundManager.Instance.PlaySE("������");
        UIManager.Instance.DownEnemyCount();
        UIManager.Instance.UpdateEnemyCountText();
        isDie = true;
        StopAllCoroutines();
        StartCoroutine(DieGorlia());
    }
    IEnumerator DieGorlia()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Instantiate(dieeEffect, transform.position + new Vector3(0, 0.5f,0), Quaternion.identity);
        Instantiate(ItemPrefab, transform.position + new Vector3(0, 0.5f,0), Quaternion.identity);
    }
    public void TimeStop()
    {
    
        if (!stopping)
        {

            stopping = true;
            Time.timeScale = 0.5f;
            StartCoroutine(Stop());
        }
    }
    IEnumerator Stop()
    {
        if (isDie)
        {
            yield break;
        }
        yield return new WaitForSecondsRealtime(stopTime);
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(slowTime);

        Time.timeScale = 1;
        stopping = false;
    }
    private void OnDisable()
    {
    
        Time.timeScale = 1;
    }
}
