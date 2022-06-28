using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StageSet : MonoBehaviour
{
    private Vector3 originPos = Vector3.zero;
    private void OnEnable()
    {
        originPos = transform.position;
        TextMove();
    }
    void TextMove()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(transform.position.y - 1000f, 1f));
        seq.Append(transform.DOMoveY(transform.position.y + Random.Range(100, 300), 1f));
        seq.Append(transform.DOMoveY(transform.position.y - Random.Range(100, 300), 1f));
        seq.Append(transform.DOMoveY(originPos.y, 2f));
        seq.AppendCallback(() => transform.gameObject.SetActive(false));
    }
}
