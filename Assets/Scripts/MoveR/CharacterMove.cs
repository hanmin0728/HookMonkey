using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CharacterMove : MonoBehaviour
{
    [SerializeField]
    private Transform debughitpoint;

    [SerializeField] private float _moveMaxSpeed = 3f;
    [SerializeField] private float _runMaxSpeed = 10f;
    [SerializeField] private float _rotateMoveSpeed = 80f;
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _turnSpeed = 80f;
    [SerializeField] private int _maxHp = 100;

    [Header("∞®º”, ∞°º”")]
    [SerializeField] private float _acceleration = 50f;
    [SerializeField] private float _deAcceleration = 50f;


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

    [Header("¿¸≈ı∞¸∑√")]
    public TrailRenderer AtkTrailRender = null;
    public CapsuleCollider AtkCapsuleCollider = null;

    [SerializeField]
    private GameObject banana;

    public bool isHook;
    public bool isJump;
    public bool isAttack;
    public int damage = 1;

    private Vector3 hookShotPosition;
    private State state;

    private enum State
    {
        Normal,
        HookShotFlyingPlayer
    }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        currnetItem = FindObjectOfType<ItemController>();
        state = State.Normal;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        durability = currnetItem.item.durability;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        switch (state)
        {
            case State.Normal:
               // HandleCharacterLock();
                HandleHookshotStart();
                MovementInput(new Vector3(h, 0f, v).normalized);
                //   HandleCharacterMovenet();
                break;
            case State.HookShotFlyingPlayer:
              //  HandleCharacterLock();
                HandleHookshotMovement();
                break;
            default:
                break;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
            if (isJump)
            {
                Jump();
            }
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
        //_rigid.AddForce(velocity, ForceMode.Acceleration);

        ChangeBody();
    }

    public void MovementInput(Vector3 movementInput)
    {
        if (movementInput.sqrMagnitude > 0)
        {
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

    public void Jump()
    {
        if (IsGround())
        {
            _rigid.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            isJump = false;
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

    bool IsGround()
    {
        Vector3 pos = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);
        Vector3 size = _collider.bounds.size * 0.2f;
        Collider[] collider = Physics.OverlapBox(pos, size, Quaternion.identity);
        foreach (var col in collider)
        {
            if (col.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }

    public void Atk()
    {
        if (isAttack)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            isAttack = true;
            banana.tag = "PlayerAtk";
            StartCoroutine(Attack());
            if (durability <= 0)
            {
                damage = 0;
            }
            else
            {
                damage = 1;
            }
            //Debug.Log(durability);
        }
    }
    IEnumerator Attack()
    {
        _animator.SetTrigger("Attack");
        durability--;
        isAttack = true;
        AtkTrailRender.enabled = true;
        AtkCapsuleCollider.enabled = true;
        yield return new WaitForSeconds(2f);
        AtkTrailRender.enabled = false;
        AtkCapsuleCollider.enabled = false;
        isAttack = false;
        banana.tag = "Untagged";

    }
    public void ChangeWeapon()
    {
        durability = 15;
    }
    public void PickItem()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(transform.position, transform.forward);
        //Debug.DrawRay(transform.position, transform.forward * 5, Color.red);

        if (Physics.Raycast(ray, out raycastHit, 5f, itemLayerMask))
        {
            _pickTxt.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                ItemPickUp obj = raycastHit.collider.GetComponent<ItemPickUp>();
                Debug.Log(obj);
                obj.PickUp();
                Debug.Log("æ∆¿Ã≈€ »πµÊ");
            }
        }
        else
        {
            _pickTxt.gameObject.SetActive(false);
        }
    }

    private void HandleHookshotStart()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit))
            {
                //hit º∂æ≈
                debughitpoint.position = raycastHit.point;
                hookShotPosition = raycastHit.point;
                state = State.HookShotFlyingPlayer;
            }
        }
    }
    private void HandleHookshotMovement()
    {
        Vector3 hookShotDir = (hookShotPosition - transform.position).normalized;
        float hookShotSpeed = Vector3.Distance(transform.position, hookShotPosition);
        float hookshotSpeedMultiplier = 2f;
        MovementInput(hookShotDir * hookShotSpeed * hookshotSpeedMultiplier  * Time.deltaTime);

        float reachedHookshotPositionDistance = 1f;
        if (Vector3.Distance(transform.position, hookShotPosition) < reachedHookshotPositionDistance)
        {
            state = State.Normal;
        }
    }
}
