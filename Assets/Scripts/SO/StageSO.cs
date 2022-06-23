using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Stage/Create New Stage")]
public class StageSO : ScriptableObject
{
    public float limitTime; //���ѽð�
    public GameObject enemyPrefab; //�� 
    public int enemyCount = 5; // �������� �� �� ����
}
