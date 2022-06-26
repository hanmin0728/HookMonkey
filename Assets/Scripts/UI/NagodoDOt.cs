using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NagodoDOt : MonoBehaviour
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
        seq.Append(transform.DOMoveY(transform.position.y + Random.Range(200f, 300f), 0.5f));
        seq.Append(transform.DOMoveY(transform.position.y - Random.Range(200f, 300f), 0.5f));
        seq.Append(transform.DOMoveY(originPos.y, 0.5f));
        seq.AppendCallback(() => transform.gameObject.SetActive(false));
    }
}
