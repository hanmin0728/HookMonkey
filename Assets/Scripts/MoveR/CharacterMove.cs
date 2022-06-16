using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private float _moveMaxSpeed = 3f;
    [SerializeField] private float _runMaxSpeed = 10f;
    [SerializeField] private float _rotateMoveSpeed = 80f;
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _turnSpeed = 80f;
    [SerializeField] private int _maxHp = 100;

    [Header("감속, 가속")]
    [SerializeField] private float _acceleration = 50f;
    [SerializeField] private float _deAcceleration = 50f;

    // public UnityEvent<float> OnVelocityChange; //플레이어 속도가 바뀔때 실행될 이벤트

    private Rigidbody _rigid;
    private Collider _collider;

    private Vector3 _currentDir = Vector3.zero;
    private float _currentVelocity = 3f;

    private bool _isRun = false;

    Vector3 posTarget = Vector3.zero;

    private Animator _animator;

    private ItemController currnetItem;

    [SerializeField]
    private int durability = 0;

    [SerializeField]
    private LayerMask itemLayerMask;

    [SerializeField]
    private Text _pickTxt;

    [Header("전투관련")]
    public TrailRenderer AtkTrailRender = null;
    public CapsuleCollider AtkCapsuleCollider = null;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        currnetItem = FindObjectOfType<ItemController>();
    }
    private void Start()
    {
        durability = currnetItem.item.durability;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        MovementInput(new Vector3(h, 0f, v).normalized);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        UpGround();
        Atk();
        PickItem();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _currentDir;
        velocity.x *= _currentVelocity;
        velocity.y = _rigid.velocity.y;
        velocity.z *= _currentVelocity;

        _rigid.velocity = velocity;
        ChangeBody();
    }

    public void MovementInput(Vector3 movementInput)
    {
        if (movementInput.sqrMagnitude > 0)
        {
            // Define static 으로 만들기
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0f;

            Vector3 right = new Vector3(forward.z, 0f, -forward.x);

            Vector3 targetDir = forward * movementInput.z + right * movementInput.x;

            Vector3 currentDir = _currentDir;
            currentDir.y = 0f;

            if (Vector3.Dot(currentDir, targetDir) < 0)
            {
                _currentVelocity = 0f;
            }

            _currentDir = Vector3.RotateTowards(_currentDir, targetDir, _rotateMoveSpeed * Time.deltaTime, 1000f);
            _currentDir.Normalize();
        }
        _currentVelocity = CalculateSpeed(movementInput);
    }


    private float CalculateSpeed(Vector3 movementInput)
    {
        if (movementInput.sqrMagnitude > 0f)
        {
            _currentVelocity += _acceleration * Time.deltaTime;
            _animator.SetBool("Walk", true);
        }

        else
        {
            _currentVelocity -= _deAcceleration * Time.deltaTime;
            _animator.SetBool("Walk", false);
        }

        return Mathf.Clamp(_currentVelocity, 0f, _isRun ? _runMaxSpeed : _moveMaxSpeed);
    }

    private void ChangeBody()
    {
        if (_currentVelocity > 0f)
        {
            Vector3 newForward = _rigid.velocity;
            newForward.y = 0f;
            transform.forward = Vector3.Lerp(transform.forward, newForward.normalized, _turnSpeed * Time.deltaTime);
        }
    }

    public void CameraFrontBody()
    {
        if (_currentVelocity > 0f) return;
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0f;
        transform.forward = forward;
    }

    public void ChangeRunState(bool state)
    {
        _isRun = state;
    }

    public void Jump()
    {
        if (IsGround())
        {
            _rigid.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
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
        Debug.DrawRay(posTarget, Vector3.down * 2f, Color.red);

        if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
        {
            // 임의의 목표 벡터에 높이 값 추가
            posTarget.y = infoRayCast.point.y;
        }
    }


    bool IsGround()
    {
        Vector3 pos = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);
        Vector3 size = _collider.bounds.size * 0.5f;
        return Physics.OverlapBox(pos, size, Quaternion.identity).Length > 1;
    }

    public void Atk()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(Attack());
            if (durability <= 0)
            {
                Debug.Log("무기를 교체하세요 적에게 데미지를 입히실수 없습니다");
            }
            Debug.Log(durability);
        }
    }
    IEnumerator Attack()
    {
        _animator.SetTrigger("Attack");
        durability--;

        AtkTrailRender.enabled = true;
        AtkCapsuleCollider.enabled = true;
        yield return new WaitForSeconds(3f);
        AtkTrailRender.enabled = false;
        AtkCapsuleCollider.enabled = false;
    }
    public void ChangeWeapon()
    {
        Debug.Log("뚜뚜뚜");
        durability = 15;
    }
    public void PickItem()
    {
        //Collider[] cubesInsideZone;

        //cubesInsideZone = Physics.OverlapSphere(this.transform.position, 2f);

        //foreach (var cube in cubesInsideZone)
        //{
        //    Debug.Log(cube);
        //    if (cube == null)
        //    {
        //        return;
        //    }
        //    _pickTxt.gameObject.SetActive(true);
        //    if (Input.GetKeyDown(KeyCode.F))
        //    {
        //        ItemPickUp obj = cube.GetComponent<ItemPickUp>();
        //        obj.PickUp();
        //        Debug.Log(obj);

        //        //Debug.Log("아이템 획득");
        //    }

        //}

        RaycastHit raycastHit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);

        if (Physics.Raycast(ray, out raycastHit, 5f, itemLayerMask))
        {
            _pickTxt.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                ItemPickUp obj = raycastHit.collider.GetComponent<ItemPickUp>();
                Debug.Log(obj);
                obj.PickUp();
                Debug.Log("아이템 획득");
            }
        }
        else
        {
            _pickTxt.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 2f);
        //if (_collider == null) _collider = GetComponent<Collider>();

        //Gizmos.color = Color.red;
        //Vector3 pos = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);
        //Vector3 size = Vector3.one * 0.1f;
        //Gizmos.DrawWireCube(pos, size);
    }

}
