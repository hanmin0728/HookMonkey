using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    //�ذ� ����
    public enum GorilaState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }

    //�ذ� �⺻ �Ӽ�
    [Header("�⺻ �Ӽ�")]
    //�ذ� �ʱ� ����
    public GorilaState gorilaState = GorilaState.None;
    //�ذ� �̵� �ӵ�
    public float spdMove = 1f;
    //�ذ��� �� Ÿ��
    public GameObject targetCharactor = null;
    //�ذ��� �� Ÿ�� ��ġ���� (�Ź� �� ã������)
    public Transform targetTransform = null;
    //�ذ��� �� Ÿ�� ��ġ(�Ź� �� ã����)
    public Vector3 posTarget = Vector3.zero;

    //�ذ� �ִϸ��̼� ������Ʈ ĳ�� 
    private Animation gorilaAnimation = null;
    //�ذ� Ʈ������ ������Ʈ ĳ��
    private Transform gorilaTransform = null;

    [Header("�ִϸ��̼� Ŭ��")]
    public AnimationClip IdleAnimClip = null;
    public AnimationClip MoveAnimClip = null;
    public AnimationClip AtkAnimClip = null;
    public AnimationClip DamageAnimClip = null;
    public AnimationClip DieAnimClip = null;

    [Header("�����Ӽ�")]
    //�ذ� ü��
    public int hp = 1;
    //�ذ� ���� �Ÿ�
    public float AtkRange = 1.5f;
    //�ذ� �ǰ� ����Ʈ
    public GameObject effectDamage = null;
    //�ذ� ���� ����Ʈ
    public GameObject effectDie = null;

    private SkinnedMeshRenderer skinnedMeshRenderer = null;


    void OnAtkAnmationFinished()
    {
        Debug.Log("Atk Animation finished");
    }

    void OnDmgAnmationFinished()
    {
        Debug.Log("Dmg Animation finished");
        gorilaState = GorilaState.Idle;
    }

    void OnDieAnmationFinished()
    {
        Debug.Log("Die Animation finished");

        //���� ���� �̺�Ʈ 
        Instantiate(effectDie, gorilaTransform.position + new Vector3(0f, 3f, 0f), Quaternion.identity);

        //���� ���� 
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


    // Start is called before the first frame update
    void Start()
    {
        //ó�� ���� ������
        gorilaState = GorilaState.Idle;

        //�ִϸ���, Ʈ������ ������Ʈ ĳ�� : �������� ã�� ������ �ʰ�
        gorilaAnimation = GetComponent<Animation>();
        gorilaTransform = GetComponent<Transform>();

        //�ִϸ��̼� Ŭ�� ��� ��� ����
        gorilaAnimation[IdleAnimClip.name].wrapMode = WrapMode.Loop;
        gorilaAnimation[MoveAnimClip.name].wrapMode = WrapMode.Loop;
        gorilaAnimation[AtkAnimClip.name].wrapMode = WrapMode.Once;
        gorilaAnimation[DamageAnimClip.name].wrapMode = WrapMode.Once;

        //�ִϸ��̼� ���� ���� ũ�� �ø�
        gorilaAnimation[DamageAnimClip.name].layer = 10;
        gorilaAnimation[DieAnimClip.name].wrapMode = WrapMode.Once;
        gorilaAnimation[DieAnimClip.name].layer = 10;

        //���� �ִϸ��̼� �̺�Ʈ �߰�
        OnAnimationEvent(AtkAnimClip, "OnAtkAnmationFinished");
        OnAnimationEvent(DamageAnimClip, "OnDmgAnmationFinished");
        OnAnimationEvent(DieAnimClip, "OnDieAnmationFinished");

        //��Ų�Ž� ĳ��
        skinnedMeshRenderer = gorilaTransform.Find("low_botton").GetComponent<SkinnedMeshRenderer>();
    }

    /// <summary>
    /// �ذ� ���¿� ���� ������ �����ϴ� �Լ� 
    /// </summary>
    void CkState()
    {
        switch (gorilaState)
        {
            case GorilaState.Idle:
                //�̵��� ���õ� RayCast��
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

    // Update is called once per frame
    void Update()
    {
        CkState();
        AnimationCtrl();
    }

    /// <summary>
    /// �ذ� ���°� ��� �� �� ���� 
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
    /// �ذ� ���°� �̵� �� �� �� 
    /// </summary>
    void setMove()
    {
        //����� ������ �� ������ ���� 
        Vector3 distance = Vector3.zero;
        //��� ������ �ٶ󺸰� ���� �ִ��� 
        Vector3 posLookAt = Vector3.zero;

        //�ذ� ����
        switch (gorilaState)
        {
            //�ذ��� ���ƴٴϴ� ���
            case GorilaState.Move:
                //���� ���� ��ġ ���� ���ΰ� �ƴϸ�
                if (posTarget != Vector3.zero)
                {
                    //��ǥ ��ġ���� �ذ� �ִ� ��ġ ���� ���ϰ�
                    distance = posTarget - gorilaTransform.position;

                    //���࿡ �����̴� ���� �ذ��� ��ǥ�� �� ���� ���� ���� 
                    if (distance.magnitude < AtkRange)
                    {
                        //��� ���� �Լ��� ȣ��
                        StartCoroutine(setWait());
                        //���⼭ ����
                        return;
                    }

                    //��� ������ �ٶ� �� ����. ���� ����
                    posLookAt = new Vector3(posTarget.x,
                                            //Ÿ���� ���� ���� ��찡 ������ y�� üũ
                                            gorilaTransform.position.y,
                                            posTarget.z);
                }
                break;
            //ĳ���͸� ���ؼ� ���� ���ƴٴϴ�  ���
            case GorilaState.GoTarget:
                //��ǥ ĳ���Ͱ� ���� ��
                if (targetCharactor != null)
                {
                    //��ǥ ��ġ���� �ذ� �ִ� ��ġ ���� ���ϰ�
                    distance = targetCharactor.transform.position - gorilaTransform.position;
                    //���࿡ �����̴� ���� �ذ��� ��ǥ�� �� ���� ���� ���� 
                    if (distance.magnitude < AtkRange)
                    {
                        //���ݻ��·� �����մ�.
                        gorilaState = GorilaState.Atk;
                        //���⼭ ����
                        return;
                    }
                    //��� ������ �ٶ� �� ����. ���� ����
                    posLookAt = new Vector3(targetCharactor.transform.position.x,
                                            //Ÿ���� ���� ���� ��찡 ������ y�� üũ
                                            gorilaTransform.position.y,
                                            targetCharactor.transform.position.z);
                }
                break;
            default:
                break;

        }

        //�ذ� �̵��� ���⿡ ũ�⸦ ���ְ� ���⸸ ����(normalized)
        Vector3 direction = distance.normalized;

        //������ x,z ��� y�� ���� �İ� ���Ŷ� ����
        direction = new Vector3(direction.x, 0f, direction.z);

        //�̵��� ���� ���ϱ�
        Vector3 amount = direction * spdMove * Time.deltaTime;

        //ĳ���� ��Ʈ���� �ƴ� Ʈ���������� ���� ��ǥ �̿��Ͽ� �̵�
        gorilaTransform.Translate(amount, Space.World);
        //ĳ���� ���� ���ϱ�
        gorilaTransform.LookAt(posLookAt);
    }

    /// <summary>
    /// ��� ���� ���� �� 
    /// </summary>
    /// <returns></returns>
    IEnumerator setWait()
    {
        //�ذ� ���¸� ��� ���·� �ٲ�
        gorilaState = GorilaState.Wait;
        //����ϴ� �ð��� �������� �ʰ� ����
        float timeWait = Random.Range(1f, 3f);
        //��� �ð��� �־� ��.
        yield return new WaitForSeconds(timeWait);
        //��� �� �ٽ� �غ� ���·� ����
        gorilaState = GorilaState.Idle;
    }

    /// <summary>
    /// �ִϸ��̼��� ��������ִ� �� 
    /// </summary>
    void AnimationCtrl()
    {
        //�ذ��� ���¿� ���� �ִϸ��̼� ����
        switch (gorilaState)
        {
            //���� �غ��� �� �ִϸ��̼� ��.
            case GorilaState.Wait:
            case GorilaState.Idle:
                //�غ� �ִϸ��̼� ����
                gorilaAnimation.CrossFade(IdleAnimClip.name);
                break;
            //������ ��ǥ �̵��� �� �ִϸ��̼� ��.
            case GorilaState.Move:
            case GorilaState.GoTarget:
                //�̵� �ִϸ��̼� ����
                gorilaAnimation.CrossFade(MoveAnimClip.name);
                break;
            //������ ��
            case GorilaState.Atk:
                //���� �ִϸ��̼� ����
                gorilaAnimation.CrossFade(AtkAnimClip.name);
                break;
            //�׾��� ��
            case GorilaState.Die:
                //���� ���� �ִϸ��̼� ����
                gorilaAnimation.CrossFade(DieAnimClip.name);
                break;
            default:
                break;

        }
    }

    ///<summary>
    ///�þ� ���� �ȿ� �ٸ� Trigger �Ǵ� ĳ���Ͱ� ������ ȣ�� �ȴ�.
    ///�Լ� ������ ��ǥ���� ������ ��ǥ���� �����ϰ� �ذ��� Ÿ�� ��ġ�� �̵� ��Ų�� 
    ///</summary>

    void OnCkTarget(GameObject target)
    {
        //��ǥ ĳ���Ϳ� �Ķ���ͷ� ����� ������Ʈ�� �ְ� 
        targetCharactor = target;
        //��ǥ ��ġ�� ��ǥ ĳ������ ��ġ ���� �ֽ��ϴ�. 
        targetTransform = targetCharactor.transform;

        //��ǥ���� ���� �ذ��� �̵��ϴ� ���·� ����
        gorilaState = GorilaState.GoTarget;

    }

    /// <summary>
    /// �ذ� ���� ���� ���
    /// </summary>
    void setAtk()
    {
        //�ذ�� ĳ���Ͱ��� ��ġ �Ÿ� 
        float distance = Vector3.Distance(targetTransform.position, gorilaTransform.position); //���̴�

        //���� �Ÿ����� �� ���� �Ÿ��� �־� ���ٸ� 
        if (distance > AtkRange + 0.5f)
        {
            //Ÿ�ٰ��� �Ÿ��� �־����ٸ� Ÿ������ �̵� 
            gorilaState = GorilaState.GoTarget;
        }
    }


    /// <summary>
    /// �ذ� �ǰ� �浹 ���� 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //���࿡ �ذ��� ĳ���� ���ݿ� �¾Ҵٸ�
        if (other.gameObject.CompareTag("PlayerAtk") == true)
        {
            //�ذ� ü���� 10 ���� 
            hp -= 10;
            if (hp > 0)
            {
                //�ǰ� ����Ʈ 

                Instantiate(effectDamage, other.transform.position, Quaternion.identity);

                gorilaState = GorilaState.Damage;

                //ü���� 0 �̻��̸� �ǰ� �ִϸ��̼��� ���� �ϰ� 
                gorilaAnimation.CrossFade(DamageAnimClip.name);

                //�ǰ� Ʈ���� ����Ʈ
                effectDamageTween();
            }
            else
            {
                //0 ���� ������ �ذ��� ���� ���·� �ٲپ��  
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
    /// �ǰݽ� ���� ������ ��½��½ ȿ���� �ش�
    /// </summary>
    void effectDamageTween()
    {
        //Ʈ���� ������ �� Ʈ�� �Լ��� ����Ǹ� ������ ������ �� �� �־ 
        //Ʈ�� �ߺ� üũ�� �̸� ������ ���ش�

        //��½�̴� ����Ʈ ������ �������ش�



        Color colorTo = Color.red;

        transform.DOJump(Vector3.up, 0.2f, 1, 2f);
        Vector3 amount;
        amount = transform.position;
        Vector3 randomUnit = Random.insideUnitSphere;
        randomUnit.y = 0f;
        transform.DOMove(amount - randomUnit * 10f, 2f);
        //skinnedMeshRenderer.materials[0].DOColor(colorTo, 0f).OnComplete(OnDamageTweenFinished);
        //skinnedMeshRenderer.materials[1].DOColor(colorTo, 0f).OnComplete(OnDamageTweenFinished);
    }

    /// <summary>
    /// �ǰ�����Ʈ ����� �̺�Ʈ �Լ� ȣ��
    /// </summary>
    void OnDamageTweenFinished()
    {
        //Ʈ���� ������ �Ͼ������ Ȯ���� ������ �����ش�
        skinnedMeshRenderer.material.DOColor(Color.white, 2f);
    }
}
