using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainCan : MonoBehaviour
{
    public GameObject sortation;
    public GameObject LeftBtn;
    public void SkippingDescription(bool isNext)
    {
        if (isNext)
        {
            sortation.SetActive(true);
            LeftBtn.SetActive(false);
        }
        else
        {
            sortation.SetActive(false);
            LeftBtn.SetActive(true);
        }
    }

}
