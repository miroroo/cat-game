using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Global Settings")]
    [Range(0f, 1f)] public float soundVolume = 0.75f;
    [Range(0f, 1f)] public float musicVolume = 0.75f;

    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Создаём источник для музыки
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.tag = "Music";

        // Загружаем настройки
        LoadSettings();
    }

    private void LoadSettings()
    {
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);

        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    // Проигрывание звука эффекта (для кнопок, шагов, взаимодействий)
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            // Создаём временный AudioSource для звука
            GameObject soundObject = new GameObject("TempSound");
            AudioSource tempSource = soundObject.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.volume = soundVolume;
            tempSource.tag = "SoundEffect";
            tempSource.Play();

            // Уничтожаем объект после окончания звука
            Destroy(soundObject, clip.length);
        }
    }

    // Проигрывание звука с указанной громкостью
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

    // Установка фоновой музыки
    public void SetMusic(AudioClip musicClip, float volume = -1f)
    {
        if (musicClip == null) return;

        if (musicSource.clip == musicClip && musicSource.isPlaying)
            return;

        musicSource.clip = musicClip;
        musicSource.volume = volume >= 0 ? volume : musicVolume;
        musicSource.Play();
    }

    // Изменение громкости эффектов (применяется ко всем новым звукам)
    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        PlayerPrefs.SetFloat("SoundVolume", volume);
        PlayerPrefs.Save();

        // Обновляем громкость у всех существующих SoundEffect
        UpdateAllSoundEffects();
    }

    // Изменение громкости музыки
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (musicSource != null)
            musicSource.volume = volume;

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    // Обновляем громкость у всех существующих звуков в текущей сцене
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