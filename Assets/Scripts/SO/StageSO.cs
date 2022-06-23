using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Stage/Create New Stage")]
public class StageSO : ScriptableObject
{
    public float limitTime; //제한시간
    public GameObject enemyPrefab; //적 
    public int enemyCount = 5; // 스테이지 당 적 개수
}
