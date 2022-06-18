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
            //�ش� ������Ʈ�� ���� ����޽����� �߼���. �Լ���, ����޽��� ��� ��ġ , ����޽��� ���� ���� ���� �ɼ� �ʼ��� �ʼ��� �ƴϳ� ���� 
            gameObject.SendMessageUpwards("OnCkTarget", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
}
