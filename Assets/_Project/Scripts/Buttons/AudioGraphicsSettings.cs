using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioGraphicsSettings : MonoBehaviour
{
    [Header("Настройки звука")]
    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Text soundVolumeText;
    [SerializeField] private Text musicVolumeText;

    [Header("Настройки яркости")]
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Text brightnessText;

    [Header("Значения по умолчанию")]
    [Range(0f, 1f)][SerializeField] private float defaultSoundVolume = 0.75f;
    [Range(0f, 1f)][SerializeField] private float defaultMusicVolume = 0.75f;
    [Range(0f, 1f)][SerializeField] private float defaultBrightness = 0.5f;

    private float currentSoundVolume;
    private float currentMusicVolume;
    private float currentBrightness;

    // Статические свойства для доступа из других скриптов
    public static float GlobalSoundVolume { get; private set; } = 0.75f;
    public static float GlobalMusicVolume { get; private set; } = 0.75f;

    private void Start()
    {
        LoadSettings();
        SetupUIListeners();
        ApplyAllSettings();
    }

    private void LoadSettings()
    {
        currentSoundVolume = PlayerPrefs.GetFloat("SoundVolume", defaultSoundVolume);
        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
        currentBrightness = PlayerPrefs.GetFloat("Brightness", defaultBrightness);

        // Обновляем статические переменные
        GlobalSoundVolume = currentSoundVolume;
        GlobalMusicVolume = currentMusicVolume;

        if (soundVolumeSlider != null) soundVolumeSlider.value = currentSoundVolume;
        if (musicVolumeSlider != null) musicVolumeSlider.value = currentMusicVolume;
        if (brightnessSlider != null) brightnessSlider.value = currentBrightness;

        UpdateTexts();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("SoundVolume", currentSoundVolume);
        PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
        PlayerPrefs.SetFloat("Brightness", currentBrightness);
        PlayerPrefs.Save();
    }

    private void SetupUIListeners()
    {
        if (soundVolumeSlider != null)
            soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);

        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        if (brightnessSlider != null)
            brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
    }

    private void OnSoundVolumeChanged(float value)
    {
        currentSoundVolume = value;
        GlobalSoundVolume = value; // Обновляем статическую переменную
        ApplySoundVolume();
        UpdateTexts();
        SaveSettings();
    }

    private void OnMusicVolumeChanged(float value)
    {
        currentMusicVolume = value;
        GlobalMusicVolume = value; // Обновляем статическую переменную
        ApplyMusicVolume();
        UpdateTexts();
        SaveSettings();

        // Обновляем музыку в OstStarter
        UpdateMusicSource();
    }

    private void OnBrightnessChanged(float value)
    {
        currentBrightness = value;
        ApplyBrightness();
        UpdateTexts();
        SaveSettings();
    }

    private void ApplySoundVolume()
    {
        // Находим все AudioSource в текущей сцене
        AudioSource[] allSounds = FindObjectsOfType<AudioSource>(true);
        foreach (AudioSource source in allSounds)
        {
            if (source.CompareTag("SoundEffect"))
            {
                source.volume = currentSoundVolume;
            }
        }
    }

    private void ApplyMusicVolume()
    {
        // Находим все AudioSource в текущей сцене
        AudioSource[] allSounds = FindObjectsOfType<AudioSource>(true);
        foreach (AudioSource source in allSounds)
        {
            if (source.CompareTag("Music"))
            {
                source.volume = currentMusicVolume;
            }
        }
    }

    // Обновляем источник музыки в OstStarter
    private void UpdateMusicSource()
    {
        if (OstStarter.Instance != null)
        {
            OstStarter.Instance.SetMusicVolume(currentMusicVolume);
        }
    }

    private void ApplyBrightness()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float brightness = currentBrightness;
            mainCamera.backgroundColor = new Color(brightness, brightness, brightness, 1f);
        }
    }

    private void UpdateTexts()
    {
        if (soundVolumeText != null)
            soundVolumeText.text = Mathf.RoundToInt(currentSoundVolume * 100) + "%";

        if (musicVolumeText != null)
            musicVolumeText.text = Mathf.RoundToInt(currentMusicVolume * 100) + "%";

        if (brightnessText != null)
            brightnessText.text = Mathf.RoundToInt(currentBrightness * 100) + "%";
    }

    private void ApplyAllSettings()
    {
        ApplySoundVolume();
        ApplyMusicVolume();
        ApplyBrightness();
        UpdateMusicSource();
    }

    public float GetSoundVolume() => currentSoundVolume;
    public float GetMusicVolume() => currentMusicVolume;
    public float GetBrightness() => currentBrightness;
}