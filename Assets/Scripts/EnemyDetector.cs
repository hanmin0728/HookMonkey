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
            //해당 오브젝트의 위로 샌드메시지를 발송한. 함수명, 샌드메시지 출발 위치 , 샌드메시지 받을 곳에 대한 옵션 필수냐 필수가 아니냐 설정 
            gameObject.SendMessageUpwards("OnCkTarget", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
}
