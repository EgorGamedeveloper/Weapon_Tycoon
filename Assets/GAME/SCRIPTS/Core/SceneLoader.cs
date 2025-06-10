using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Статический метод для удобства вызова без Instance
    public static void LoadScene(string sceneName)
    {
        if (Instance != null)
        {
            Instance.StartCoroutine(Instance.LoadSceneAsync(sceneName));
        }
        else
        {
            Debug.LogError("SceneLoader instance not found!");
        }
    }

    // Корутина для асинхронной загрузки сцены с возможностью расширить функционал
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Опционально: показать экран загрузки через UIManager
        //if (UIManager.Instance != null)
        //{
        //    UIManager.Instance.ShowLoadingScreen?.Invoke();
        //}
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        // Пока сцена грузится, можно обновлять прогресс UI (op.progress)
        while (!op.isDone)
        {
            // Пример: обновить прогрессбар на UI, если надо
            yield return null;
        }
        // Сцена загружена, можно скрыть экран загрузки
        //if (UIManager.Instance != null)
        //{
        //    UIManager.Instance.HideLoadingScreen?.Invoke();
        //}
    }
}
