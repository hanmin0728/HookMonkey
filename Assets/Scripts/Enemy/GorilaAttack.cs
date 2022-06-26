using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorilaAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IHittable>().OnDamage?.Invoke();
        }
    }
}
