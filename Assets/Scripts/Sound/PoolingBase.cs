using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingBase : MonoBehaviour
{
    [SerializeField] protected bool isTimePool = true;
    [SerializeField] protected float poolTime;
    [SerializeField] protected PoolObject poolIndex;
    [SerializeField] protected PoolManager PoolManager;
    public virtual void Start()
    {

    }
    public void OnEnable()
    {
        if (!PoolManager) { PoolManager = GameManager.Instance.PoolManager; }
        transform.SetParent(PoolManager.Holder);
        if (isTimePool)
        {
            StartCoroutine(Pool());
        }
    }
    IEnumerator Pool()
    {
        yield return new WaitForSeconds(poolTime);
        transform.SetParent(PoolManager.Get(poolIndex));
        gameObject.SetActive(false);
    }
    public void InstanceaPool()
    {
        transform.SetParent(PoolManager.Get(poolIndex));
        gameObject.SetActive(false);
    }
}
