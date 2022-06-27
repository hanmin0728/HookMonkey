using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
public class MonkeyMove : MonoBehaviour, IHittable
{
    private ItemController currnetItem;

    [SerializeField]
    private Text _pickTxt;

    [SerializeField]
    private LayerMask itemLayerMask;

    public int _hp = 5;
    public int _maxHp;
    public int durability = 0;
    public int Maxdurability = 0;
    public int damage = 1;
    public bool isHook;

    public bool isOpenInven = false;
    public bool isOpenTip = false;

    private CharacterController characterController;
    private float cameraVerticalAngle;
    private float characterVelocityY;
    private Vector3 characterVelocityMomentum;
    private Vector3 hookShotPosition;
    public Transform debughitpoint;
    public Transform hookShotTransform;
    private Vector3 moveDir;

    private float hookShotSize;
    public LayerMask layerMask;

    public GameObject inven;
    public GameObject tip;

    public UnityEvent dieEvent;
    public GameObject damageEffect;

    public GameObject dieEffect;

    #region 카메라 관련
    [Header("카메라 관련")]
    private const float NORMAL_FOV = 60f;
    private const float HOOKSHOT_FOV = 100f;
    private Camera playerCamera;
    private CameraFOV cameraFOV;
    public float mouseSensitivity = 1f;
    #endregion

    #region 플레이어 상태
    private State state;
    private enum State
    {
        Normal,
        HookShotFlyingPlayer,
        HookShotTrown
    }
    #endregion 

    private Animator _animator;

    [SerializeField] private UnityEvent IHittable;

    UnityEvent IHittable.OnDamage { get => IHittable; set { } }

    public bool isDamaged = false;

    //public Slider hpBar;
    public Slider nagodoBar;

    public GameObject nagodoDot;

    public CameraShake cameraShake;

    public GameObject waringDurabillty;

    public Image hpImage;
    public Image nagodoImage;

    InventoryManager inventoryManager;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        playerCamera = transform.Find("CameraHolder").GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        cameraFOV = playerCamera.GetComponent<CameraFOV>();
        currnetItem = FindObjectOfType<ItemController>();
        // Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        state = State.Normal;
        hookShotTransform.gameObject.SetActive(false);
    }
    private void Start()
    {
        durability = currnetItem.item.durability;
        _maxHp = 5;
        Maxdurability = durability;
    }
    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                HandleCharacterLock();
                HandleCharacterMovement();
                HandleHookshotStart();
                break;
            case State.HookShotTrown:
                HandleHookShotThrow();
                HandleCharacterLock();
                HandleCharacterMovement();
                break;
            case State.HookShotFlyingPlayer:
                HandleCharacterLock();
                HandleHookshotMovement();
                break;
            default:
                break;
        }
        PickItem();
        OpenTip(isOpenTip);
        OpenInventory(isOpenInven);
        DownDieCheck();
        if (_hp <= 0)
        {
            Invoke("Die", 0.5f);
            //Die();
        }
        DownCheck();
        OpenCursor();
    }
    public void DownCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!characterController.isGrounded)
            {
                Time.timeScale = 0.15f;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1f;
        }
    }
    public void DownDieCheck()
    {
        if (transform.position.y <= -30f)
        {
            Debug.Log("PlayerDIe");
            Invoke("Die", 0.5f);
        }
    }
    private void HandleCharacterLock()
    {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");

        transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);

        cameraVerticalAngle -= lookY * mouseSensitivity;

        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);

    }
    private void HandleCharacterMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float moveSpeed = 5f;

        Vector3 characterVelocity = transform.right * moveX * moveSpeed + transform.forward * moveZ * moveSpeed;

        if (characterController.isGrounded)
        {
            characterVelocityY = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float jumpSpeed = 15f;
                characterVelocityY = jumpSpeed;
            }
        }
        float gravityDownForce = -60f;
        characterVelocityY += gravityDownForce * Time.deltaTime;

        characterVelocity.y = characterVelocityY;

        characterVelocity += characterVelocityMomentum;

        characterController.Move(characterVelocity * Time.deltaTime);

        if (characterVelocityMomentum.magnitude > 0f)
        {
            float momentumDrag = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if (characterVelocityMomentum.magnitude < 0f)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }

        moveDir = characterVelocity;
        if (new Vector2(characterVelocity.x, characterVelocity.z).sqrMagnitude < 0.1f)
        {
            _animator.SetBool("Walk", false);
        }
        else
        {
            _animator.SetBool("Walk", true);
        }
    }
    private void ResetGravityEffect()
    {
        characterVelocityY = 0f;
    }
    private void HandleHookshotStart()
    {
        if (TestInputDownHookshot())
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit, layerMask))
            {
                debughitpoint.position = raycastHit.point;
                hookShotPosition = raycastHit.point;
                hookShotSize = 0;
                hookShotTransform.gameObject.SetActive(true);
                hookShotTransform.localScale = Vector3.zero;
                state = State.HookShotTrown;
                isHook = true;
            }
        }
    }
    private void HandleHookShotThrow()
    {
        hookShotTransform.LookAt(hookShotPosition);
        float hookshotTrownSpeed = 250f;
        hookShotSize += hookshotTrownSpeed * Time.deltaTime;
        hookShotTransform.localScale = new Vector3(1, 1, hookShotSize);

        if (hookShotSize >= Vector3.Distance(transform.position, hookShotPosition))
        {
            state = State.HookShotFlyingPlayer;
            cameraFOV.SetCameraFov(HOOKSHOT_FOV);
        }
    }
    private void HandleHookshotMovement()
    {
        hookShotTransform.LookAt(hookShotPosition);

        Vector3 hookShotDir = (hookShotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 40f;


        float hookShotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookShotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 2f;
        characterController.Move(hookShotDir * hookShotSpeed * hookshotSpeedMultiplier * Time.deltaTime);


        float reachedHookshotPositionDistance = 1f;
        float atackRange = 3f;
        if (Vector3.Distance(transform.position, hookShotPosition) < reachedHookshotPositionDistance)
        {
            StopHookShot();
        }
        else if (Vector3.Distance(transform.position, hookShotPosition) < atackRange)
        {
            _animator.SetTrigger("Attack");
            if (isHook)
            {

                MonkeyAttack();

            }
            else
            {
                return;
            }
            // StartCoroutine(Attack());
        }
        if (TestInputDownHookshot())
        {
            StopHookShot();
        }
        if (TestInputJump())
        {
            float momentExtraSpeed = 7f;
            characterVelocityMomentum = hookShotDir * hookShotSpeed * momentExtraSpeed;
            float jumpSpeed = 40f;
            characterVelocityMomentum += Vector3.up * jumpSpeed;
            state = State.Normal;
            ResetGravityEffect();
            hookShotTransform.gameObject.SetActive(false);
        }
    }
    private void StopHookShot()
    {
        state = State.Normal;
        ResetGravityEffect();
        hookShotTransform.gameObject.SetActive(false);
        cameraFOV.SetCameraFov(NORMAL_FOV);
        isHook = false;
    }
    private bool TestInputDownHookshot()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    private bool TestInputJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    public void MonkeyAttack()
    {
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        //if (isHook)
        //{
        //    yield break;
        //}
        if (durability == 5)
        {
            waringDurabillty.SetActive(true);
        }
        if (durability <= 0)
        {
            damage = 0;
        }
        else
        {
            damage = 1;
        }

        yield return null;
    }
    public void OpenCursor()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Cursor.visible = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Cursor.visible = false;
        }
    }
    public void OpenInventory(bool _isActive)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpenInven = _isActive ? false : true;
            inventoryManager.ListItems();
            inven.SetActive(isOpenInven);
        }

    }   
    public void OpenTip(bool _isActive)
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            isOpenTip = _isActive ? false : true;
            tip.SetActive(isOpenTip);
        }

    }
    public void ChangeWeapon(Item item)
    {
        Debug.Log("실행됨");
        nagodoDot.SetActive(true);
        durability = 15;
        nagodoImage.fillAmount = 1;
        //nagodoBar.value = 15;
    }
    public void PickItem()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(transform.position, transform.forward);
        //Debug.DrawRay(transform.position, transform.forward * 5, Color.red);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out raycastHit, 50f, itemLayerMask))
            //sif (Physics.Raycast(ray, out raycastHit, 5f, itemLayerMask))
        {
            _pickTxt.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                ItemPickUp obj = raycastHit.collider.GetComponent<ItemPickUp>();
                obj.PickUp();
            }
        }
        else
        {
            _pickTxt.gameObject.SetActive(false);
        }
    }
    public void Damage()
    {
        StartCoroutine(DamageCoroutine());
    }
    IEnumerator DamageCoroutine()
    {
        if (isDamaged)
        {
            yield break;
        }
        isDamaged = true;
        Debug.Log(_hp);
        _hp--;

        //hpBar.value -= 1;

        hpImage.fillAmount -= 1f / _maxHp; // 적데미지나누기맥스ㅇ헤이히피
        Debug.Log(1 / _maxHp);
        //   Debug.Log(hpBar.value);
        //hpBar.value = Mathf.Lerp(hpBar.value, _hp, Time.deltaTime * 10);
        //cameraShake.ShakeCam(.15f, 1f);
        Instantiate(damageEffect, transform.position, Quaternion.identity);
        PlayerHitFeedBack();
        yield return new WaitForSeconds(7f);
        isDamaged = false;
    }

    public void Die()
    {
        dieEvent?.Invoke();
        Instantiate(dieEffect, transform.position, Quaternion.identity);
        Time.timeScale = 0f;
    }
    public void PlayerHitFeedBack()
    {
        transform.DOJump(Vector3.up, 0.2f, 1, 2f);
        Vector3 amount = transform.position;
        Vector3 randomUnit = Random.insideUnitSphere;
        randomUnit.y = 0f;

        transform.DOMove(amount - randomUnit * 10f, 2f);
    }

    //private void HandleHP()
    //{
    //    hpBar.value = Mathf.Lerp(hpBar.value, _hp, Time.deltaTime * 10);
    //}
    //private void HandleNagodo()
    //{
    //    nagodoBar.value = Mathf.Lerp(nagodoBar.value, durability, Time.deltaTime * 10);
    //}
}
