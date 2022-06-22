using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public string targetTag = string.Empty;

    //트리거 안에 들어오면 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) == true)
        {
            gameObject.SendMessageUpwards("OnCkTarget", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
}
