using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    // Источники звука для музыки и эффектов (можно больше, например, разные каналы)
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // Коллекция аудиоклипов, доступных по имени (например, заполнить через инспектор или Resources)
    public List<AudioClip> musicClips;
    public List<AudioClip> soundEffects;

    private Dictionary<string, AudioClip> musicMap;   // для быстрого поиска клипа по имени
    private Dictionary<string, AudioClip> sfxMap;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Инициализация словарей аудио для доступа по ключу-именам
        musicMap = new Dictionary<string, AudioClip>();
        foreach (var clip in musicClips)
        {
            musicMap[clip.name] = clip;
        }
        sfxMap = new Dictionary<string, AudioClip>();
        foreach (var clip in soundEffects)
        {
            sfxMap[clip.name] = clip;
        }
    }

    public void PlayMusic(string clipName)
    {
        if (musicMap.ContainsKey(clipName))
        {
            AudioClip clip = musicMap[clipName];
            if (musicSource.isPlaying && musicSource.clip == clip)
            {
                return; // уже играет этот трек
            }
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music clip not found: " + clipName);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySound(string clipName)
    {
        if (sfxMap.ContainsKey(clipName))
        {
            sfxSource.PlayOneShot(sfxMap[clipName]);
        }
        else
        {
            Debug.LogWarning("SFX clip not found: " + clipName);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
