using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject diePanel;
    public GameObject deletePopup;  

    private InventoryItemController _item;

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
}
