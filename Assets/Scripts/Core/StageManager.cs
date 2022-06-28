using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoSingleton<StageManager>
{
    public StageSO currentStageSO;

    public int currentEnemyCount;
    public int startEnemyCount;

  //  public Text enemyCount;

    protected override void Awake()
    {
        startEnemyCount = currentStageSO.enemyCount;
        currentEnemyCount = startEnemyCount;
    }
    private void Update()
    {
        if (currentEnemyCount <= 0)
        {
            StageClear();
        }

    }
    private void StageClear()
    {
        UIManager.Instance.StageClearText();

    }
}
