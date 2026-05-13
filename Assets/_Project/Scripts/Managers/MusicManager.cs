using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Global Settings")]
    [Range(0f, 1f)] public float soundVolume = 0.75f;
    [Range(0f, 1f)] public float musicVolume = 0.75f;

    private AudioSource musicSource;
    private AudioClip currentMusicClip; // Запоминаем текущую музыку

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // НЕ создавать повторно AudioSource
        musicSource = GetComponent<AudioSource>();

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;

        LoadSettings();
    }

    private void LoadSettings()
    {
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);

        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    // Проверка, играет ли указанная музыка
    public bool IsMusicPlaying(AudioClip clip)
    {
        if (musicSource == null) return false;
        return musicSource.isPlaying && musicSource.clip == clip;
    }

    // Проверка, играет ли любая музыка
    public bool IsAnyMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying;
    }

    // Установка фоновой музыки
    public void SetMusic(AudioClip musicClip, float volume = -1f)
    {
        if (musicClip == null) return;

        // Если та же музыка уже играет - не перезапускаем
        if (musicSource.clip == musicClip && musicSource.isPlaying)
        {
            Debug.Log("Same music is already playing, skipping.");
            return;
        }

        musicSource.clip = musicClip;
        musicSource.volume = volume >= 0 ? volume : musicVolume;
        musicSource.Play();
        currentMusicClip = musicClip;

        Debug.Log($"Starting new music: {musicClip.name}");
    }

    // Остальные методы без изменений...
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            GameObject soundObject = new GameObject("TempSound");
            AudioSource tempSource = soundObject.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.volume = soundVolume;
            tempSource.tag = "SoundEffect";
            tempSource.Play();
            Destroy(soundObject, clip.length);
        }
    }

    public void PlaySound(AudioClip clip, float volumeMultiplier)
    {
        if (clip != null)
        {
            GameObject soundObject = new GameObject("TempSound");
            AudioSource tempSource = soundObject.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.volume = soundVolume * volumeMultiplier;
            tempSource.tag = "SoundEffect";
            tempSource.Play();
            Destroy(soundObject, clip.length);
        }
    }

    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        PlayerPrefs.SetFloat("SoundVolume", volume);
        PlayerPrefs.Save();
        UpdateAllSoundEffects();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (musicSource != null)
            musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    private void UpdateAllSoundEffects()
    {
        AudioSource[] allSources = FindObjectsOfType<AudioSource>(true);
        foreach (AudioSource source in allSources)
        {
            if (source.CompareTag("SoundEffect") && source != musicSource)
            {
                source.volume = soundVolume;
            }
        }
    }

    public float GetSoundVolume() => soundVolume;
    public float GetMusicVolume() => musicVolume;
    public void PauseMusic() => musicSource.Pause();
    public void ResumeMusic() => musicSource.UnPause();
}