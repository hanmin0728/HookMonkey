using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StartDot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Re());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Re()
    {
        while(true)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f));
            seq.Append(transform.DOScale(Vector3.one, 0.5f));
            yield return new WaitForSeconds(3f);
            seq.AppendCallback(() => seq.Kill());
        }
    }
}
