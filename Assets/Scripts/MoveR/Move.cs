using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 5f;
    public float BodyRotateSpd = 2.0f;
    public float VelocityChangeSpd = 0.1f;
    private Vector3 CurrentVelocitySpd = Vector3.zero;
    private float verticalSpd = 0f;
    Vector3 forward;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PMove();
        setGravity();
        BodyDirectChange();
    }

    void PMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        forward = Camera.main.transform.forward;
        forward.y = 0.0f;

        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        Vector3 pos = h * right + v * forward;

        Vector3 _vecTemp = new Vector3(0f, verticalSpd, 0f);
        rb.velocity = (pos * speed) + _vecTemp;
    }

    void BodyDirectChange()
    {
        Vector3 retVelocity = rb.velocity;
        retVelocity.y = 0;
        CurrentVelocitySpd = Vector3.Lerp(CurrentVelocitySpd, retVelocity, VelocityChangeSpd * Time.fixedDeltaTime);

        if (CurrentVelocitySpd.magnitude > 0.1f)
        {
            transform.forward = Vector3.Lerp(transform.forward, retVelocity, BodyRotateSpd * Time.deltaTime);
        }
    }

    void setGravity()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, 1.1f))
        {
            verticalSpd = 0f;
        }
        else
        {
            verticalSpd -= 9.8f * Time.deltaTime;
        }
    }
}