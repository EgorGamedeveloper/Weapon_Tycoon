using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    public ItemSlot[] itemSlots;

   public void UpdateSlots()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].UpdateSlot(Inventory.Instance.carriedResources[i].GetComponent<ItemData>());
        }
    }
}
