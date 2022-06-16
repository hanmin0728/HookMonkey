using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    Item item;
    private CharacterMove _characterMove;
    public Button removeButton;
    private void Awake()
    {
        _characterMove = FindObjectOfType<CharacterMove>();

        GetComponent<Button>().onClick.AddListener(() => UseItem());
    }
    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
    }
    public void AddItem(Item newItem)
    {
        item = newItem;
    }
    public void UseItem()
    {
        Debug.Log("AA");
        _characterMove.ChangeWeapon();
        Destroy(gameObject);
    }
}
