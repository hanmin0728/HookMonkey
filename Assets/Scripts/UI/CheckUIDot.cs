using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CheckUIDot : MonoBehaviour
{
    Sequence seq;
    private void OnEnable()
    {
        seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one * 1.5f, 0.2f));
    }
    private void OnDisable()
    {
        transform.localScale = Vector3.one;
        seq?.Kill();   
    }
}
