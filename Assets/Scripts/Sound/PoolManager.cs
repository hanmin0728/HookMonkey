using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region 프로피터
    public Transform Holder { get { return holder; } }
    #endregion
    [SerializeField] private Transform holder;
    [SerializeField] private Transform[] poolTrans;
    public Transform Get(PoolObject index)
    {
        return poolTrans[(int)index];
    }
}
public enum PoolObject
{
    sound,
    effect,
    other
};