using UnityEngine;
using System;

[Serializable]  // Класс данных, чтобы можно было сохранять в JSON
public class GameData
{
    public int money;
    public int level;
    public string currentTask;
    // ... любые другие данные игрового прогресса
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public GameData currentData = new GameData();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Попытаться загрузить данные при инициализации
        LoadGame();
    }

    public void SaveGame()
    {
        // Собрать текущие данные игры (например, из других менеджеров или объектов)
        //currentData.money = /* получить текущие деньги игрока, например, GameManager или др. */;
        //currentData.level = /* текущий уровень или прогресс */;
        //currentData.currentTask = /* текущее задание, если есть */;
        // ... собрать остальной прогресс

        // Сериализовать в JSON (можно использовать JsonUtility)
        string json = JsonUtility.ToJson(currentData);
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
        Debug.Log("Game saved locally.");

        // Также отправить данные в облако через PlatformSDKManager (если авторизован/возможно)
        PlatformSDKManager.Instance?.SaveToCloud(json);
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            string json = PlayerPrefs.GetString("SaveData");
            currentData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded from local save.");
        }
        else
        {
            currentData = new GameData(); // новое сохранение, значения по умолчанию
            Debug.Log("No local save found, starting new game data.");
        }

        // Применить загруженные данные к игре:
        ApplyLoadedData();
    }

    public void ApplyLoadedData()
    {
        // Применяем данные currentData к текущему состоянию игры.
        // Например, установить количество денег UIManager и GameManager.
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMoney(currentData.money);
            // Обновить другие элементы UI, если нужно
        }
        // Установить прогресс игрока в GameManager или других объектах:
        // GameManager.Instance.SetPlayerLevel(currentData.level);
        // GameManager.Instance.SetCurrentTask(currentData.currentTask);
        // ... и т.д.
    }
}
