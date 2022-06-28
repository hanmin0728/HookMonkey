using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class ClearDot : MonoBehaviour
{
    private Vector3 originPos = Vector3.zero;
    public GameObject clearChang;
    private void OnEnable()
    {
        originPos = transform.position;
        TextMove();
    }
    void TextMove()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(transform.position.y + Random.Range(200f, 300f), 0.5f));
        seq.Append(transform.DOMoveY(transform.position.y - Random.Range(200f, 300f), 0.5f));
        seq.Append(transform.DOMoveY(originPos.y, 0.5f));
        seq.AppendCallback(() => Time.timeScale = 0f);
        seq.AppendCallback(() => clearChang.SetActive(true));
        seq.AppendCallback(() => UIManager.Instance.ClearTimeText());
    }
}
