using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public PoolManager PoolManager { get { return poolManager; } }
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private Transform player;
    public Transform Player { get { return player; } }


    private float timeScale = 1f;

    public static float TimeScale
    {
        get
        {
            if (Instance != null)
            {
                return Instance.timeScale;
            }
            else
            {
                return 0f;
            }

        }
        set
        {
            Instance.timeScale = Mathf.Clamp(value, 0, 1);
        }
    }
    protected override void Awake()
    {
        base.Awake();
       poolManager = FindObjectOfType<PoolManager>();
    }
}
