using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIManager : MonoSingleton<UIManager>
{
    public GameObject diePanel;
    public GameObject deletePopup;

    private InventoryItemController _item;

    public GameObject clearTmp;
    public Text enemyCountTxt;

    public Text clearTxt;

    public Timer timer;
    public float cleartime;
    public float timetime;
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
        Cursor.visible = true;
    }
    public void ClearTimeText()
    {
        timetime = StageManager.Instance.currentStageSO.limitTime;
        cleartime = timetime -= timer.setTime;
        clearTxt.text = ($"클리어까지 걸린 시간: {Mathf.Round(cleartime).ToString()}s");
        //clearTxt.DOText($"Clear하는데까지 걸린시간{ cleartime}", 0.5f);
    }
    public void CursorOn()
    {
        Cursor.visible = true;
    } 
    public void CursorOff()
    {
        Cursor.visible = false;
    }


}
