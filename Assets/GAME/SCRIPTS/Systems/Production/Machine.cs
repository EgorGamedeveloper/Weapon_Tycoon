using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Machine : MonoBehaviour
{
    [Header("Настройки станка")]
    public RecipeData currentRecipe; // Активный рецепт производства
    public float processingTime = 5f; // Базовое время производства одного изделия (секунды)
    public int upgradeLevel = 1;     // Уровень улучшения станка (начинается с 1)

    [Header("Состояние станка")]
    private Dictionary<ResourceData, int> storedResources = new Dictionary<ResourceData, int>();
    private int outputInventory = 0; // Сколько готовых изделий находится на выдаче

    private bool isProcessing = false; // Идёт ли сейчас процесс производства

    public void SetRecipe(RecipeData newRecipe)
    {
        currentRecipe = newRecipe;
        storedResources.Clear();
        outputInventory = 0;
    }

    // Методы загрузки ресурсов и выдачи продукта могут вызываться скриптами-триггерами зон:
    public void AddResource(Player player)
    {
        if (currentRecipe == null) return;

        bool added = false;
        foreach (var req in currentRecipe.requirements)
        {
            int current = storedResources.ContainsKey(req.resource) ? storedResources[req.resource] : 0;
            while (current < req.amount && player.inventory.HasItem(req.resource))
            {
                player.inventory.RemoveItem(req.resource, 1);
                current++;
                storedResources[req.resource] = current;
                added = true;
            }
        }

        if (added)
        {
            Debug.Log($"Станок получил ресурсы для рецепта: {currentRecipe.recipeName}");
            TryStartProduction();
        }
    }

    private void TryStartProduction()
    {
        if (isProcessing || currentRecipe == null) return;

        foreach (var req in currentRecipe.requirements)
        {
            if (!storedResources.ContainsKey(req.resource) || storedResources[req.resource] < req.amount)
                return;
        }

        StartCoroutine(ProcessProduction());
    }

    private IEnumerator ProcessProduction()
    {
        if (currentRecipe == null) yield break;
        isProcessing = true;
        // Учитываем ускорение от уровня улучшения: например, каждый уровень снижает время на 10%
        float effectiveTime = processingTime / (1 + 0.1f * (upgradeLevel - 1));
        Debug.Log("Начало производства изделия...");
        yield return new WaitForSeconds(effectiveTime); // имитация работы станка
        // По окончании времени производства:
        foreach (var req in currentRecipe.requirements)
        {
            storedResources[req.resource] -= req.amount;
        }
        outputInventory += 1;  // добавляем одно готовое изделие в буфер выхода
        Debug.Log("Изделие произведено и готово к выдаче");
        isProcessing = false;
        TryStartProduction();
        // В реальном проекте можно оповестить систему (например, через событие), что продукт готов.
    }

    public void CollectProduct(Player player)
    {
        if (outputInventory > 0)
        {
            outputInventory -= 1;
            // Добавляем продукт в инвентарь игрока
            player.inventory.AddItem(currentRecipe.outputProduct, 1);
            Debug.Log($"Игрок забрал готовый предмет: {currentRecipe.outputProduct.productName}");
            // Опционально: если хотим визуально отображать предмет на выходе, 
            // можно вместо мгновенного добавления заспавнить префаб предмета, 
            // а уже поднятие предмета игроком добавит его в инвентарь.
        }
    }

    // Пример обработки входа игрока в зону триггера ввода сырья
    private void OnTriggerEnter(Collider other)
    {
        // Предполагается, что этот метод прикреплен к коллайдеру зоны ввода.
        // Проверяем, что вошёл игрок (по тегу или по компоненту Player)
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            AddResource(player);
        }
        // Аналогично можно обрабатывать вход ресурса на конвейере:
        // Например, если other содержит ItemObject и его ResourceData присутствует в рецепте,
        // то автоматически засасывать его:
        // ItemObject item = other.GetComponent<ItemObject>();
        // if(item != null && currentRecipe.HasResource(item.data)) { Destroy(item.gameObject); /* увеличить хранилище */ }
    }

    // Пример обработки входа игрока в зону триггера вывода продукции
    private void OnTriggerExit(Collider other)
    {
        // Этот метод вызван на зоне вывода, когда игрок выходит из неё.
        // Мы можем выдать предмет в момент выхода или входа, в зависимости от дизайна.
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            CollectProduct(player);
        }
    }
}
