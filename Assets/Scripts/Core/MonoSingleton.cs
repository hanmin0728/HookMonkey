using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//찾을때마다 생성하는 방식
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    var _newGameobj = new GameObject(typeof(T).ToString());
                    _instance = _newGameobj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance = null)
        {
            _instance = this as T;
        }
        //씬이 바뀌어도 사라지지 않게 객체로 지정
        //최상위 객체여야한다
        DontDestroyOnLoad(gameObject);
    }
}