using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform stackPoint;
    public int maxCapacity = 5;
    public List<GameObject> carriedResources = new List<GameObject>();
    
    public static Inventory Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Второй инвентарь найден. Удаляю себя.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Сколько ещё предметов можно добавить.
    /// </summary>
    public int GetRemainingCapacity()
    {
        return maxCapacity - carriedResources.Count;
    }

    /// <summary>
    /// Пытается добавить предмет. 
    /// Вернёт true, если добавлено, false — если места нет.
    /// </summary>
    public bool TryAddItem(GameObject item)
    {
        if (carriedResources.Count >= maxCapacity)
            return false;

        item.transform.SetParent(stackPoint);
        item.transform.localPosition = new Vector3(0, carriedResources.Count * 0.5f, 0);
        carriedResources.Add(item);
        

        return true;
    }
}

