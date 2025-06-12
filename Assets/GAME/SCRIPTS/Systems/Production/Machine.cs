using UnityEngine;
using System.Collections;

public class Machine : MonoBehaviour
{
    [Header("Настройки станка")]
    public ResourceData inputType;   // Тип ресурса, принимаемого на вход
    public ProductData outputType;   // Тип продукта, производимого на выходе
    public float processingTime = 5f; // Время производства одного изделия (секунды)
    public int upgradeLevel = 1;     // Уровень улучшения станка (начинается с 1)

    [Header("Состояние станка")]
    private int storedInput = 0;     // Сколько сырья загружено и ждёт переработки
    private int outputInventory = 0; // Сколько готовых изделий находится на выдаче

    private bool isProcessing = false; // Идёт ли сейчас процесс производства

    // Методы загрузки ресурсов и выдачи продукта могут вызываться скриптами-триггерами зон:
    public void AddResource(Player player)
    {
        // Проверяем, есть ли у игрока нужный ресурс в инвентаре
        if (player.inventory.HasItem(inputType))
        {
            player.inventory.RemoveItem(inputType, 1);  // изымаем 1 единицу ресурса из инвентаря
            storedInput += 1;
            Debug.Log($"Станок получил ресурс: {inputType.resourceName}");
            // Если станок не занят, запускаем производство
            if (!isProcessing) StartCoroutine(ProcessProduction());
        }
    }

    private IEnumerator ProcessProduction()
    {
        if (storedInput <= 0) yield break; // нет сырья – выход
        isProcessing = true;
        // Учитываем ускорение от уровня улучшения: например, каждый уровень снижает время на 10%
        float effectiveTime = processingTime / (1 + 0.1f * (upgradeLevel - 1));
        Debug.Log("Начало производства изделия...");
        yield return new WaitForSeconds(effectiveTime); // имитация работы станка
        // По окончании времени производства:
        storedInput -= 1;      // расходуем одну единицу сырья
        outputInventory += 1;  // добавляем одно готовое изделие в буфер выхода
        Debug.Log("Изделие произведено и готово к выдаче");
        isProcessing = false;
        // Если после производства в станке ещё осталось сырьё, запускаем следующий цикл автоматически
        if (storedInput > 0) StartCoroutine(ProcessProduction());
        // В реальном проекте можно оповестить систему (например, через событие), что продукт готов.
    }

    public void CollectProduct(Player player)
    {
        if (outputInventory > 0)
        {
            outputInventory -= 1;
            // Добавляем продукт в инвентарь игрока
            player.inventory.AddItem(outputType, 1);
            Debug.Log($"Игрок забрал готовый предмет: {outputType.productName}");
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
        // Например, если other содержит ItemObject с ResourceData == inputType, 
        // то автоматически засасывать его:
        // ItemObject item = other.GetComponent<ItemObject>();
        // if(item != null && item.data == inputType) { Destroy(item.gameObject); storedInput++; }
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
