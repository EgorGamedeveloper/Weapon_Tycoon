using UnityEngine;
using System.Collections.Generic;

public class WorkerManager : MonoBehaviour
{
    public static WorkerManager Instance; // Singleton для глобального доступа (опционально)
    public int maxWorkers = 5;            // Ограничение по количеству рабочих
    public GameObject workerPrefab;       // Префаб рабочего для инстанциации
    private List<Worker> workers = new List<Worker>();

    void Awake()
    {
        Instance = this;
    }

    // Метод найма рабочего
    public bool HireWorker(Vector3 spawnPosition, Machine assignMachine, Transform storagePoint)
    {
        if (workers.Count >= maxWorkers)
        {
            Debug.Log("Достигнуто максимальное число рабочих.");
            return false;
        }
        // Создаем нового рабочего
        GameObject obj = Instantiate(workerPrefab, spawnPosition, Quaternion.identity);
        Worker newWorker = obj.GetComponent<Worker>();
        if (newWorker != null)
        {
            workers.Add(newWorker);
            // Назначаем рабочему задачу (например, прикрепляем к указанному станку)
            if (assignMachine != null)
            {
                newWorker.AssignToMachine(assignMachine, storagePoint);
            }
            Debug.Log($"Нанят новый рабочий. Всего рабочих: {workers.Count}");
            return true;
        }
        return false;
    }

    // Метод увольнения рабочего (например, удаление последнего)
    public void RemoveWorker(Worker worker)
    {
        if (workers.Contains(worker))
        {
            workers.Remove(worker);
            Destroy(worker.gameObject);
            Debug.Log($"Рабочий удален. Осталось рабочих: {workers.Count}");
        }
    }

    // Применить ускоряющий буст всем рабочим
    public void BoostAllWorkers(float multiplier, float duration)
    {
        foreach (Worker w in workers)
        {
            w.ApplySpeedMultiplier(multiplier, duration);
        }
        Debug.Log($"Применен буст x{multiplier} к скорости всех рабочих на {duration} сек.");
    }

    // Сохранить данные о рабочих (пример метода, можно использовать при сохранении игры)
    public List<WorkerSaveData> GetWorkersSaveData()
    {
        List<WorkerSaveData> dataList = new List<WorkerSaveData>();
        foreach (Worker w in workers)
        {
            WorkerSaveData data = new WorkerSaveData();
            data.position = w.transform.position;
            //data.assignedMachineID = w.GetAssignedMachineID();
            // ^ Предполагается, что у Machine есть ID или индекс, чтобы восстановить привязку.
            dataList.Add(data);
        }
        return dataList;
    }
}

// Структура для сохранения данных о рабочем
[System.Serializable]
public class WorkerSaveData
{
    public Vector3 position;
    public int assignedMachineID;
    // Можно сохранять и другие параметры (например, скорость, если прокачивается, и т.п.)
}
