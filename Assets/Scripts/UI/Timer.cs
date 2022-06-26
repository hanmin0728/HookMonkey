using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;
    [SerializeField]
    public StageSO stageLimitTimeTimme;
    private float setTime;
    private void Awake()
    {
        setTime = stageLimitTimeTimme.limitTime;
    }
  
    private void Update()
    {
        if (setTime > 0)
        {

        setTime -= Time.deltaTime;
        }
        else if (setTime <= 0)
        {
            Time.timeScale = 0f;
            Debug.Log("timeout");

        }
        timer.text = Mathf.Round(setTime).ToString();
    }
}
