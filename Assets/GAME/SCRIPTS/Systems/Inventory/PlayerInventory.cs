using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InventorySlot
{
    public ScriptableObject itemData; // может быть ResourceData или ProductData
    public int count;
    public InventorySlot(ScriptableObject data, int cnt) { itemData = data; count = cnt; }
}

public class PlayerInventory : MonoBehaviour
{
    public List<InventorySlot> items = new List<InventorySlot>();
    public int capacity = 20; // максимальное количество различных предметов (для примера)

    // Ссылка на UI-менеджер инвентаря для обновления отображения
    public InventoryUI uiManager;

    // Добавить предмет в инвентарь
    public void AddItem(ScriptableObject itemData, int amount)
    {
        // Ищем слот с таким же типом
        InventorySlot slot = items.Find(s => s.itemData == itemData);
        if (slot != null)
        {
            slot.count += amount;
        }
        else
        {
            // Если нет слота и есть место, создаем новый
            if (items.Count < capacity)
            {
                items.Add(new InventorySlot(itemData, amount));
            }
            else
            {
                Debug.LogWarning("Инвентарь переполнен, не удалось добавить предмет");
                return;
            }
        }
        Debug.Log($"Добавлено: {amount} x {itemData.name} в инвентарь.");
        // Обновляем UI после изменения инвентаря
        uiManager?.Refresh(items);
    }

    // Удалить предмет (использовать при загрузке в станок или продаже)
    public bool RemoveItem(ScriptableObject itemData, int amount)
    {
        InventorySlot slot = items.Find(s => s.itemData == itemData);
        if (slot != null && slot.count >= amount)
        {
            slot.count -= amount;
            if (slot.count == 0)
            {
                items.Remove(slot);
            }
            Debug.Log($"Удалено: {amount} x {itemData.name} из инвентаря.");
            uiManager?.Refresh(items);
            return true;
        }
        return false;
    }

    // Проверить наличие предмета
    public bool HasItem(ScriptableObject itemData, int amount = 1)
    {
        InventorySlot slot = items.Find(s => s.itemData == itemData);
        return (slot != null && slot.count >= amount);
    }
}
