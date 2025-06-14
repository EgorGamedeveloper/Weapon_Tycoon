using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour
{
    public float baseSpeed = 3f;    // Базовая скорость движения
    public int carryCapacity = 1;   // Сколько единиц груза может нести
    private float speedMultiplier = 1f; // Текущий мультипликатор скорости (бусты)
    private bool isCarrying = false;
    private ResourceData carryingResource = null;
    private ProductData carryingProduct = null;

    // Ссылки на цели (устанавливаются менеджером или при назначении задания)
    private Machine targetMachine;
    private Transform targetStorage;  // точка склада или продажи

    // Метод назначения на станок для обслуживания
    public void AssignToMachine(Machine machine, Transform storagePoint)
    {
        targetMachine = machine;
        targetStorage = storagePoint;
        // Можно сразу задать начальную задачу, например, принести ресурс к станку
    }

    void Update()
    {
        // Простейшая логика движения к цели
        if (targetMachine == null) return; // если не назначен на задачу, ничего не делаем

        float step = baseSpeed * speedMultiplier * Time.deltaTime;
        // Если рабочий несет ресурс -> двигаться к станку
        if (isCarrying && carryingResource != null)
        {
            // Двигаемся к станку с ресурсом
            transform.position = Vector3.MoveTowards(transform.position, targetMachine.transform.position, step);
            // Проверим расстояние до станка
            if (Vector3.Distance(transform.position, targetMachine.transform.position) < 1f)
            {
                // В зоне станка - разгружаем ресурс
                //targetMachine.AddResource(GameManager.Instance.Player);
                // ^ здесь для упрощения вызываем AddResource, 
                // но по идее рабочий сам должен загрузить, без игрока.
                // Можно вместо передачи игрока перегрузить метод AddResource(ResourceData).
                carryingResource = null;
                isCarrying = false;
                // После разгрузки можно запланировать взять готовый продукт, если есть
            }
        }
        else
        {
            // Если не несем ресурс, идем на склад за ресурсом
            if (!isCarrying)
            {
                // Двигаемся к складу (или месту появления ресурсов)
                transform.position = Vector3.MoveTowards(transform.position, targetStorage.position, step);
                if (Vector3.Distance(transform.position, targetStorage.position) < 1f)
                {
                    // Прибыли на склад: берем ресурс (условно, проверка наличия)
                    if (targetMachine.currentRecipe != null && targetMachine.currentRecipe.requirements.Length > 0)
                    {
                        carryingResource = targetMachine.currentRecipe.requirements[0].resource;
                    }
                    else
                    {
                        carryingResource = null;
                    }
                    isCarrying = true;
                    Debug.Log("Рабочий взял ресурс со склада.");
                }
            }
        }
        // Аналогично можно реализовать перенос готового продукта к складу/рынку.
    }

    // Применение ускорения работы на duration секунд
    public void ApplySpeedMultiplier(float multiplier, float duration)
    {
        StartCoroutine(Boost(multiplier, duration));
    }
    private IEnumerator Boost(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
    }
}
