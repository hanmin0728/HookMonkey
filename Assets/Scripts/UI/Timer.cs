using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;
    [SerializeField]
    public StageSO stageLimitTimeTimme;
    public float setTime;
    MonkeyMove m;
    private void Awake()
    {

        m = FindObjectOfType<MonkeyMove>();
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
            m.dieText.text = ($"Death reason: Timeout");
            UIManager.Instance.OnDiePanel();
            //m.Die();
            m.Invoke("Die", 0.5f);
            Time.timeScale = 0f;
        }
        timer.text = ($"Å¸ÀÌ¸Ó: { Mathf.Round(setTime).ToString()}s");
    }
}
