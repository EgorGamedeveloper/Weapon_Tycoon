using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Синглтон (опционально, если UIManager один на все сцены)
    public static UIManager Instance { get; private set; }

    // Ссылки на основные панели UI (назначаются в инспекторе)
    public GameObject mainMenuPanel;
    public GameObject gameHUDPanel;
    public GameObject pauseMenuPanel;
    public GameObject gameOverPanel;
    // Примеры UI элементов
    public Text moneyText;       // текстовое поле для отображения денег
    public Text taskText;        // текст для текущего задания/цели

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        // Если планируем использовать UIManager в нескольких сценах:
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // Изначально покажем главное меню, спрятав остальные панели
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        // Показать главное меню, скрыть остальные окна
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gameHUDPanel != null) gameHUDPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void ShowGameHUD()
    {
        // Показать HUD во время игры
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameHUDPanel != null) gameHUDPanel.SetActive(true);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        // Игра уже на паузе через GameManager, просто отобразим меню паузы
    }

    public void HidePauseMenu()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    // Пример метода обновления UI-элемента:
    public void UpdateMoney(int amount)
    {
        if (moneyText != null)
        {
            moneyText.text = amount.ToString();
        }
    }

    public void UpdateTask(string taskDescription)
    {
        if (taskText != null)
        {
            taskText.text = taskDescription;
        }
    }

    // Методы, привязанные к кнопкам UI:
    public void OnPlayButton()
    {
        // Нажата кнопка "Играть" на главном меню
        GameManager.Instance.StartGame();
        ShowGameHUD();
    }

    public void OnPauseButton()
    {
        // Нажата кнопка "Пауза" в HUD
        GameManager.Instance.PauseGame();
    }

    public void OnResumeButton()
    {
        // Нажата кнопка "Продолжить" на экране паузы
        GameManager.Instance.ResumeGame();
    }

    public void OnRestartButton()
    {
        // Нажата кнопка "Заново" на экране завершения игры
        GameManager.Instance.StartGame();
        // Скрываем экран завершения и показываем HUD заново
        ShowGameHUD();
    }
}
