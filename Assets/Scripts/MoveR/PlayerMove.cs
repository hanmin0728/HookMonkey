using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();

    }
    private float verticalSpd = 0f;
    Vector3 forward;
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);
        Vector3 pos = h * right + v * forward;

        Vector3 _vecTemp = new Vector3(0f, verticalSpd, 0f);
        rb.velocity = (pos * speed) + _vecTemp;
    }





}
