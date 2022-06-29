using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    
    public Item item;
    public void PickUp()
    {
        SoundManager.Instance.PlaySE("æ∆¿Ã≈€");
        InventoryManager.Instance.Add(item);
        Destroy(gameObject);
    }
    private void OnMouseDown()
    {
        PickUp();
    }
}
