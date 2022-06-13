using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public enum GorilaState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }

    [Header("�⺻ �Ӽ�")]
    public GorilaState gorilaState = GorilaState.None;
    public float spdMove = 1f;
    public GameObject targetCharactor = null;
    public Transform targetTransform = null;
    public Vector3 posTarget = Vector3.zero;

    private Animation gorilaAnimation = null;
    private Transform gorilaTransform = null;

    [Header("�ִϸ��̼� Ŭ��")]
    public AnimationClip IdleAnimClip = null;
    public AnimationClip MoveAnimClip = null;
    public AnimationClip AtkAnimClip = null;
    public AnimationClip DamageAnimClip = null;
    public AnimationClip DieAnimClip = null;

    [Header("�����Ӽ�")]
    public int hp = 1;
    public float AtkRange = 1.5f;
    public GameObject effectDamage = null;
    public GameObject effectDie = null;

    private SkinnedMeshRenderer skinnedMeshRenderer = null;


    void OnAtkAnmationFinished()
    {
    }

    void OnDmgAnmationFinished()
    {
        gorilaState = GorilaState.Idle;
    }

    void OnDieAnmationFinished()
    {

        Instantiate(effectDie, gorilaTransform.position + new Vector3(0f, 1.5f, 0f), Quaternion.identity);

        Destroy(gameObject);
    }

    /// <summary>
    /// �ִϸ��̼� �̺�Ʈ�� �߰����ִ� ��. 
    /// </summary>
    /// <param name="clip">�ִϸ��̼� Ŭ�� </param>
    /// <param name="funcName">�Լ��� </param>
    void OnAnimationEvent(AnimationClip clip, string funcName)
    {
        //�ִϸ��̼� �̺�Ʈ�� ����� �ش�
        AnimationEvent retEvent = new AnimationEvent();
        //�ִϸ��̼� �̺�Ʈ�� ȣ�� ��ų �Լ���
        retEvent.functionName = funcName;
        //�ִϸ��̼� Ŭ�� ������ �ٷ� ������ ȣ��
        retEvent.time = clip.length - 0.1f;
        //�� ������ �̺�Ʈ�� �߰� �Ͽ���
        clip.AddEvent(retEvent);
    }


    void Start()
    {
        gorilaState = GorilaState.Idle;

        gorilaAnimation = GetComponent<Animation>();
        gorilaTransform = GetComponent<Transform>();

        gorilaAnimation[IdleAnimClip.name].wrapMode = WrapMode.Loop;
        gorilaAnimation[MoveAnimClip.name].wrapMode = WrapMode.Loop;
        gorilaAnimation[AtkAnimClip.name].wrapMode = WrapMode.Once;
        gorilaAnimation[DamageAnimClip.name].wrapMode = WrapMode.Once;

        gorilaAnimation[DamageAnimClip.name].layer = 10;
        gorilaAnimation[DieAnimClip.name].wrapMode = WrapMode.Once;
        gorilaAnimation[DieAnimClip.name].layer = 10;

        OnAnimationEvent(AtkAnimClip, "OnAtkAnmationFinished");
        OnAnimationEvent(DamageAnimClip, "OnDmgAnmationFinished");
        OnAnimationEvent(DieAnimClip, "OnDieAnmationFinished");

    }

    /// <summary>
    /// ���� ���¿� ���� ������ �����ϴ� �Լ� 
    /// </summary>
    void CkState()
    {
        switch (gorilaState)
        {
            case GorilaState.Idle:
                setIdle();
                break;
            case GorilaState.GoTarget:
            case GorilaState.Move:
                setMove();
                break;
            case GorilaState.Atk:
                setAtk();
                break;
            default:
                break;
        }
    }

    void Update()
    {
        CkState();
        AnimationCtrl();
    }

    /// <summary>
    /// ���� ���°� ��� �� �� ���� 
    /// </summary>
    void setIdle()
    {
        if (targetCharactor == null)
        {
            posTarget = new Vector3(gorilaTransform.position.x + Random.Range(-10f, 10f),
                                    gorilaTransform.position.y + 1000f,
                                    gorilaTransform.position.z + Random.Range(-10f, 10f)
                );
            Ray ray = new Ray(posTarget, Vector3.down);
            RaycastHit infoRayCast = new RaycastHit();
            if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
            {
                posTarget.y = infoRayCast.point.y;
            }
            gorilaState = GorilaState.Move;
        }
        else
        {
            gorilaState = GorilaState.GoTarget;
        }
    }

    /// <summary>
    /// ���� ���°� �̵� �� �� �� 
    /// </summary>
    void setMove()
    {
        Vector3 distance = Vector3.zero;
        Vector3 posLookAt = Vector3.zero;

        switch (gorilaState)
        {
            case GorilaState.Move:
                if (posTarget != Vector3.zero)
                {
                    distance = posTarget - gorilaTransform.position;

                    if (distance.magnitude < AtkRange)
                    {
                        StartCoroutine(setWait());
                        return;
                    }

                    posLookAt = new Vector3(posTarget.x,
                                            gorilaTransform.position.y,
                                            posTarget.z);
                }
                break;
            case GorilaState.GoTarget:
                if (targetCharactor != null)
                {
                    distance = targetCharactor.transform.position - gorilaTransform.position;
                    if (distance.magnitude < AtkRange)
                    {
                        gorilaState = GorilaState.Atk;
                        return;
                    }
                    posLookAt = new Vector3(targetCharactor.transform.position.x,
                                            gorilaTransform.position.y,
                                            targetCharactor.transform.position.z);
                }
                break;
            default:
                break;

        }

        Vector3 direction = distance.normalized;

        direction = new Vector3(direction.x, 0f, direction.z);

        Vector3 amount = direction * spdMove * Time.deltaTime;

        gorilaTransform.Translate(amount, Space.World);
        gorilaTransform.LookAt(posLookAt);
    }

    /// <summary>
    /// ��� ���� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator setWait()
    {
        gorilaState = GorilaState.Wait;
        float timeWait = Random.Range(1f, 3f);
        yield return new WaitForSeconds(timeWait);
        gorilaState = GorilaState.Idle;
    }

    /// <summary>
    /// �ִϸ��̼��� ��������ִ� �Լ�
    /// </summary>
    void AnimationCtrl()
    {
        switch (gorilaState)
        {
            case GorilaState.Wait:
            case GorilaState.Idle:
                gorilaAnimation.CrossFade(IdleAnimClip.name);
                break;
            case GorilaState.Move:
            case GorilaState.GoTarget:
                gorilaAnimation.CrossFade(MoveAnimClip.name);
                break;
            case GorilaState.Atk:
                gorilaAnimation.CrossFade(AtkAnimClip.name);
                break;
            case GorilaState.Die:
                gorilaAnimation.CrossFade(DieAnimClip.name);
                break;
            default:
                break;

        }
    }

    ///<summary>
    ///�þ� ���� �ȿ� �ٸ� Trigger �Ǵ� ĳ���Ͱ� ������ ȣ�� �ȴ�.
    ///�Լ� ������ ��ǥ���� ������ ��ǥ���� �����ϰ� ���� Ÿ�� ��ġ�� �̵� ��Ų�� 
    ///</summary>
    void OnCkTarget(GameObject target)
    {
        targetCharactor = target;
        targetTransform = targetCharactor.transform;
        gorilaState = GorilaState.GoTarget;

    }

    /// <summary>
    /// ���� ���� ���� ���
    /// </summary>
    void setAtk()
    {
        float distance = Vector3.Distance(targetTransform.position, gorilaTransform.position); //���̴�

        if (distance > AtkRange + 0.5f)
        {
            gorilaState = GorilaState.GoTarget;
        }
    }


    /// <summary>
    /// ���� �ǰ� �浹 ���� 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAtk") == true)
        {
            hp -= 10;
            if (hp > 0)
            {
                Instantiate(effectDamage, other.transform.position, Quaternion.identity);

                gorilaState = GorilaState.Damage;
                gorilaAnimation.CrossFade(DamageAnimClip.name);
                effectDamageTween();
            }
            else
            {
                gorilaState = GorilaState.Die;
                StartCoroutine(OnDie(DieAnimClip.length - 0.3f));
            }
        }
    }
    private IEnumerator OnDie(float t)
    {
        yield return new WaitForSeconds(t);
        OnDieAnmationFinished();
    }

    /// <summary>
    /// �ǰݽ� ���͸� �˹��Ѵ�
    /// </summary>
    void effectDamageTween()
    {
        transform.DOJump(Vector3.up, 0.2f, 1, 2f);
        Vector3 amount;
        amount = transform.position;
        Vector3 randomUnit = Random.insideUnitSphere;
        randomUnit.y = 0f;
        transform.DOMove(amount - randomUnit * 10f, 2f);
    }
}
