using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    public Text itemNameText;
    Item item;
    private MonkeyMove _monkeyMove;
    [SerializeField] private GameObject obj;
    bool isCasing;
    private void Awake()
    {
        _monkeyMove = FindObjectOfType<MonkeyMove>();
    }
   
    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        UIManager.Instance.deletePopup.SetActive(false);
        Destroy(gameObject);
    }
    public void AddItem(Item newItem)
    {
        item = newItem;

        if(!isCasing)
        {
            isCasing = true;
            Button but = GetComponent<Button>();
            obj = but.gameObject;
            but.onClick.AddListener(() => UseItem(newItem));
        }

        itemNameText.text = newItem.itemNaming; 
    }
    public void OnCheckDelete()
    {
        UIManager.Instance.deletePopup.SetActive(true);
        UIManager.Instance.SetItemController(this);
    }

    public void UseItem(Item newItem)
    {
        item = newItem;
        InventoryManager.Instance.Remove(item);
        _monkeyMove.ChangeWeapon(newItem);
        RemoveItem();
    }
}
