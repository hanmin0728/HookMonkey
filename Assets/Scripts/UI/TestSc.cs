using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSc : MonoBehaviour
{
    public Image jojun;

    void Start()
    {
        jojun.color = new Color(1, 1, 1, 0.75f);
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * Mathf.Infinity, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                jojun.color = new Color(1, 0, 0, 0.75f);

            }

        }
            else
            {
                jojun.color = new Color(1, 1, 1, 0.75f);

            }
    }
}
