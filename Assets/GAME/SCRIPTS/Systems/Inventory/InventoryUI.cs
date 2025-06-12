using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // Предположим, у нас есть prefab слота инвентаря в UI:
    public GameObject slotPrefab;
    public Transform contentPanel; // панель-контейнер для слотов

    public void Refresh(List<InventorySlot> items)
    {
        // Очистить текущие элементы UI:
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        // Создать UI-элемент для каждого предмета в списке
        foreach (var slot in items)
        {
            GameObject slotGO = Instantiate(slotPrefab, contentPanel);
            // Настроить изображение и текст:
            // Например:
            // slotGO.GetComponent<Image>().sprite = (slot.itemData as ItemData).icon;
            // slotGO.GetComponentInChildren<Text>().text = slot.count.ToString();
        }
    }
}
