using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEffect : MonoBehaviour
{
    public Transform parent;
    public GameObject clearEffect;
    void Start()
    {
        StartCoroutine(CreateEffect());
    }
    IEnumerator CreateEffect()
    {
        while(true)
        {
            float randX = Random.Range(2, -4);
            float randY = Random.Range(1, 4);
            Instantiate(clearEffect, new Vector3(parent.transform.position.x + randX, parent.transform.position.y + randY, parent.transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
    }
}
