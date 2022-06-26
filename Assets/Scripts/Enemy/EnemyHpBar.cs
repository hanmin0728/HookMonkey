using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyHpPrefab;

    List<Transform> enemyObjectList = new List<Transform>();
    List<GameObject> enemyHPBarList = new List<GameObject>();
    Camera enemyCam;
    void Start()
    {
        enemyCam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
