using UnityEngine;
using System;

public class PlatformSDKManager : MonoBehaviour
{
    public static PlatformSDKManager Instance { get; private set; }

    // События, на которые можно подписаться для реагирования на рекламу
    public event Action OnAdOpened;    // Интерстициальная (полноэкранная) реклама открыта
    public event Action OnAdClosed;    // Интерстициальная реклама закрыта
    public event Action OnRewardAdOpened;  // Реклама с вознаграждением открыта
    public event Action OnRewardAdClosed;  // Реклама с вознаграждением закрыта (просмотр завершён)
    public event Action OnRewardGranted;   // Пользователь заслужил награду (досмотрел рекламу)

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSDK();
    }

    void InitializeSDK()
    {
        // Инициализация SDK конкретной платформы.
#if UNITY_WEBGL
            Debug.Log("Initializing Web platform SDK...");
            // Например, инициализация веб-SDK (если требуется)
            // В WebGL можно использовать Application.ExternalCall для вызова JS функций
#elif UNITY_ANDROID || UNITY_IOS
            Debug.Log("Initializing Mobile Ads SDK...");
            // Инициализация мобильного рекламного SDK, например Unity Ads / AdMob / etc.
            // UnityAds.Initialize(gameId, testMode);
            // Подписка на колбэки рекламы (если SDK позволяет)
#else
        Debug.Log("No specific SDK initialization required for this platform.");
#endif
    }

    public void ShowInterstitialAd()
    {
#if UNITY_WEBGL
            Debug.Log("Request to show interstitial ad on Web.");
            // Вызов JavaScript-функции для показа рекламы, например:
            // Application.ExternalCall("GameSDK_ShowInterstitial");
            // Далее, JavaScript-код вызовет обратный вызов OnInterstitialAdClosed() по завершении
#elif UNITY_ANDROID || UNITY_IOS
            Debug.Log("Request to show interstitial ad on Mobile.");
            // Вызов метода показа рекламы мобильного SDK:
            // if(UnityAds.IsReady("video")) { UnityAds.Show("video"); }
            // Колбэки Unity Ads настроены так, чтобы вызвать методы OnInterstitialAdOpened/Closed
#else
        Debug.Log("Interstitial ad not supported on this platform.");
#endif
    }

    public void ShowRewardedAd()
    {
#if UNITY_WEBGL
            Debug.Log("Request to show rewarded ad on Web.");
            // Application.ExternalCall("GameSDK_ShowRewarded");
#elif UNITY_ANDROID || UNITY_IOS
            Debug.Log("Request to show rewarded ad on Mobile.");
            // if(UnityAds.IsReady("rewardedVideo")) { UnityAds.Show("rewardedVideo"); }
#else
        Debug.Log("Rewarded ad not supported on this platform.");
        // Можно сразу начислить награду или ничего не делать
#endif
    }

    // Эти методы будут вызваны из SDK или из JavaScript (через SendMessage или ExternalCall callbacks)
    // Метод, вызываемый при открытии рекламы
    public void OnInterstitialAdOpenedCallback()
    {
        Debug.Log("Interstitial Ad Opened");
        OnAdOpened?.Invoke();
        // Например, можно приостановить звук: AudioListener.pause = true;
    }
    // Метод, вызываемый при закрытии рекламы
    public void OnInterstitialAdClosedCallback()
    {
        Debug.Log("Interstitial Ad Closed");
        OnAdClosed?.Invoke();
        // Возобновить звук: AudioListener.pause = false;
    }
    public void OnRewardedAdOpenedCallback()
    {
        Debug.Log("Rewarded Ad Opened");
        OnRewardAdOpened?.Invoke();
    }
    public void OnRewardedAdClosedCallback(bool rewardReceived)
    {
        Debug.Log("Rewarded Ad Closed, reward = " + rewardReceived);
        OnRewardAdClosed?.Invoke();
        if (rewardReceived)
        {
            OnRewardGranted?.Invoke();
            // Начислить игроку награду за просмотр рекламы, например удвоение ресурсов:
            // GameManager.Instance.GrantReward();
        }
    }

    public void SaveToCloud(string jsonData)
    {
#if UNITY_WEBGL
            // Вызов JavaScript для сохранения данных, например через Web API или платформенный API
            // Application.ExternalCall("GameSDK_SaveData", jsonData);
            Debug.Log("Saving data to cloud (Web): " + jsonData);
#elif UNITY_ANDROID || UNITY_IOS
            // Использование какого-нибудь облачного сервиса, например Google Play Saved Games или собственный сервер
            Debug.Log("Saving data to cloud (Mobile): " + jsonData);
            // Например, вызвать PlayGamesPlatform.Instance.SaveGame(...) если интегрировано
#else
        Debug.Log("Cloud save not supported on this platform.");
#endif
    }

    public void LoadFromCloud()
    {
#if UNITY_WEBGL
            // Запросить данные из облака
            Debug.Log("Loading data from cloud (Web)...");
            // Application.ExternalCall("GameSDK_LoadData"); 
            // Предполагается, что JS вызовет обратный метод с данными, например OnCloudDataReceived(json)
#elif UNITY_ANDROID || UNITY_IOS
            Debug.Log("Loading data from cloud (Mobile)...");
            // Вызов соответствующего метода SDK облачных сохранений
#else
        Debug.Log("Cloud load not supported on this platform.");
#endif
    }

    // Вызывается платформой с полученными данными облака (например, через SendMessage)
    public void OnCloudDataReceived(string jsonData)
    {
        Debug.Log("Cloud data received: " + jsonData);
        if (!string.IsNullOrEmpty(jsonData))
        {
            // Передать данные SaveManager для обработки
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.currentData = JsonUtility.FromJson<GameData>(jsonData);
                SaveManager.Instance.ApplyLoadedData();
            }
        }
    }
}
