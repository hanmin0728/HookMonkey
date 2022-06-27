using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
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
}
