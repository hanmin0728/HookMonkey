using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public string targetTag = string.Empty;

    //Ʈ���� �ȿ� ������ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) == true)
        {
            gameObject.SendMessageUpwards("OnCkTarget", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
}
