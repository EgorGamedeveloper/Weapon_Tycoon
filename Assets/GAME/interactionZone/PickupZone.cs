using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupZone : InteractableObject
{
    public ItemData item;
    public List<GameObject> items = new List<GameObject>();
    public int itemCount;

    public void Start()
    {
        for (int i = 0; i < itemCount; i++)
        {
            MakeItem();
        }
    }

    public override void Interect()
    {
        Debug.Log("PickupZone.Interact()");

        // Сколько слотов свободно
        int freeSlots = Inventory.Instance.GetRemainingCapacity();

        // Добавляем не больше, чем есть в зоне и не больше, чем поместится в инвентарь
        int itemsToAdd = Mathf.Min(freeSlots, items.Count);

        // Берём itemsToAdd первых элементов и добавляем их
        for (int i = 0; i < itemsToAdd; i++)
        {
            GameObject go = items[0];        // всегда первый в списке
            bool added = Inventory.Instance.TryAddItem(go);
            if (added)
            {
                items.RemoveAt(0);
            }
            else
            {
                Debug.LogWarning("Не удалось добавить предмет в инвентарь!");
                break;
            }
        }
    }


    public void MakeItem()
    {
        GameObject obj = Instantiate(item.prefab, transform.position, Quaternion.identity);
        items.Add(obj);
        //obj.AddComponent(item);
    }

}
