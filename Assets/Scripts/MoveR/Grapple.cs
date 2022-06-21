using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public float pullSpd = 0.5f;
    public float stopDistance = 4f;
    public GameObject hookPrefab;
    public Transform shootTrm;

    H h;
    bool pulling = false;
    Rigidbody rigid;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        pulling = false;
    }
    private void Update()
    {
        if (h == null && Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            pulling = false;
            h = Instantiate(hookPrefab, shootTrm.position, Quaternion.identity).GetComponent<H>();
            h.Intiialize(this, shootTrm);
            StartCoroutine(DestroyHookAfterLifeTime());
        
        }
        else if (h != null && Input.GetMouseButtonDown(1))
        {
            DestoryHook();
        }
        if (!pulling || h == null) return;

        if (Vector3.Distance(transform.position, h.transform.position) <= stopDistance)
        {
            DestoryHook();
        }
        else
        {
            rigid.AddForce((h.transform.position - transform.position).normalized * pullSpd, ForceMode.VelocityChange);
        }
    }
    public void StartPull()
    {
        pulling = true;
    }
    private void DestoryHook()
    {
        if (h == null) return;

        pulling = false;
        Destroy(h.gameObject);
        h = null;
    }
    private IEnumerator DestroyHookAfterLifeTime()
    {
        yield return new WaitForSeconds(8f);

        DestoryHook();
    }
}
