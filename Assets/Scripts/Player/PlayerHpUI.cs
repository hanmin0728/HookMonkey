using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    
    public Slider hpBar;

    private float maxHP = 5;
    private float curHP = 5;
    float ismi;
    float damage;
    void Start()
    {
        hpBar.value = (float)curHP / (float)maxHP;    
    }

    void Update()
    {
        HandleHP();
    }
    private void HandleHP()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, ismi, Time.deltaTime * 10); 
    }
}
