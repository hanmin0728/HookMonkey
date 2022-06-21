using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H : MonoBehaviour
{
    public float hookForce = 25f;
    Rigidbody rigid;
    LineRenderer lineRenderer;
    Grapple grapple;
  public void Intiialize(Grapple grapple, Transform shootTrm)
    {
        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
       // transform.forward = shootTrm.forward;
        Camera.main.transform.forward = shootTrm.forward;
        this.grapple = grapple;
        rigid.AddForce(transform.forward * hookForce, ForceMode.Impulse);
    }

    void Update()
    {
        Vector3[] postions = new Vector3[]
        {
            transform.position,
            grapple.transform.position
        };
        lineRenderer.SetPositions(postions);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((LayerMask.GetMask("CanHook") & 1 << other.gameObject.layer) > 0)
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;

            grapple.StartPull();
        }
    }
}
