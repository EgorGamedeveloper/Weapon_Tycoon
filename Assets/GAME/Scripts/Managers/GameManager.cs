using UnityEngine;
using UnityEngine.SceneManagement;
using System;  // для использования событий Action

public class GameManager : MonoBehaviour
{
    // Синглтон-инстанс GameManager
    public static GameManager Instance { get; private set; }

    // События для важных изменений состояния игры
    public event Action OnGameStarted;
    public event Action OnGamePaused;
    public event Action OnGameResumed;
    public event Action OnGameOver;

    // Ссылки на другие менеджеры (можно установить через инспектор или FindObjectOfType)
    public UIManager uiManager;
    public AudioManager audioManager;
    public SaveManager saveManager;
    public PlatformSDKManager platformManager;

    // Текущее состояние игры (например, Playing, Paused, Stopped)
    private bool isGamePaused = false;

    void Awake()
    {
        // Реализация синглтона: если экземпляр уже есть, уничтожить новый
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject); // сохраняем между сценами

        // Инициализация (если ссылки не заданы в инспекторе, найти объекты на сцене)
        if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
        if (audioManager == null) audioManager = FindObjectOfType<AudioManager>();
        if (saveManager == null) saveManager = FindObjectOfType<SaveManager>();
        if (platformManager == null) platformManager = FindObjectOfType<PlatformSDKManager>();

        // Подписка на события платформы: напр. пауза на время рекламы
        if (platformManager != null)
        {
            platformManager.OnAdOpened += PauseGame;
            platformManager.OnAdClosed += ResumeGame;
        }
    }

    void Start()
    {
        // Начало игры – показываем главное меню через UIManager
        uiManager.ShowMainMenu();
        audioManager.PlayMusic("MainMenuTheme"); // проиграть фоновую музыку меню
    }

    public void StartGame()
    {
        // Метод запуска игровой сессии (например, по нажатию кнопки "Play")
        SceneLoader.LoadScene("GameScene"); // загрузить основную игровую сцену
        OnGameStarted?.Invoke();  // вызвать событие начала игры
        audioManager.PlayMusic("GameTheme"); // переключить музыку на игровую
    }

    public void PauseGame()
    {
        if (isGamePaused) return;
        isGamePaused = true;
        Time.timeScale = 0f;  // Остановить время игры
        OnGamePaused?.Invoke();
        uiManager.ShowPauseMenu();
    }

    public void ResumeGame()
    {
        if (!isGamePaused) return;
        isGamePaused = false;
        Time.timeScale = 1f;  // Возобновить время
        OnGameResumed?.Invoke();
        uiManager.HidePauseMenu();
    }

    public void GameOver()
    {
        // Вызывается при окончании игры/уровня
        OnGameOver?.Invoke();
        uiManager.ShowGameOverScreen();
        // Можно предложить показать рекламу за вознаграждение через PlatformSDKManager
    }
}
