using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject diePanel;
    public GameObject deletePopup;  

    private InventoryItemController _item;

    public GameObject clearTmp;
    public Text enemyCountTxt;

    private void Start()
    {
        UpdateEnemyCountText();
    }
    public void SetItemController(InventoryItemController item)
    {
        _item = item;
    }
    public void DeleteSeletedItem()
    {
        if (_item == null) return;

        _item.RemoveItem();
        _item = null;
    }
    public void OnDiePanel()
    {
        diePanel.SetActive(true);
    }

    public void UpdateEnemyCountText()
    {
        enemyCountTxt.text = ($" X{ StageManager.Instance.currentEnemyCount}"); 
    }
    public void DownEnemyCount()
    {
        StageManager.Instance.currentEnemyCount -= 1;
    }
    public void StageClearText()
    {
        clearTmp.SetActive(true);
    }
    

}
