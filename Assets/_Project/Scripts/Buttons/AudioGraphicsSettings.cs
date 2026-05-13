using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        // Загружаем настройки ДО настройки слушателей
        LoadSettings();
        SetupUIListeners();

        // Применяем все настройки после загрузки
        StartCoroutine(ApplySettingsAfterAudioManagerReady());
    }

    private System.Collections.IEnumerator ApplySettingsAfterAudioManagerReady()
    {
        // Ждём инициализации AudioManager
        while (AudioManager.Instance == null)
        {
            yield return null;
        }

        // Применяем звуковые настройки
        ApplyAudioSettings();
        ApplyAllSettings();
    }

    private void LoadSettings()
    {
        float currentSoundVolume = PlayerPrefs.GetFloat("SoundVolume", defaultSoundVolume);
        float currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
        float currentBrightness = PlayerPrefs.GetFloat("Brightness", defaultBrightness);

        if (soundVolumeSlider != null) soundVolumeSlider.value = currentSoundVolume;
        if (musicVolumeSlider != null) musicVolumeSlider.value = currentMusicVolume;
        if (brightnessSlider != null) brightnessSlider.value = currentBrightness;

        UpdateTexts();
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
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSoundVolume(value);
        else
            Debug.LogWarning("AudioManager.Instance is null! Sound volume will be applied later.");

        UpdateTexts();
    }

    private void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
        else
            Debug.LogWarning("AudioManager.Instance is null! Music volume will be applied later.");

        UpdateTexts();
    }

    private void OnBrightnessChanged(float value)
    {
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();

        ApplyBrightness(value);

        if (BrightnessManager.Instance != null)
            BrightnessManager.Instance.SetBrightness(value);

        UpdateTexts();
    }

    private void ApplyAudioSettings()
    {
        float soundVol = PlayerPrefs.GetFloat("SoundVolume", defaultSoundVolume);
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSoundVolume(soundVol);
            AudioManager.Instance.SetMusicVolume(musicVol);
        }
    }

    private void ApplyBrightness(float value)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = new Color(value, value, value, 1f);
        }
    }

    private void UpdateTexts()
    {
        if (soundVolumeText != null)
        {
            float soundVol = PlayerPrefs.GetFloat("SoundVolume", defaultSoundVolume);
            soundVolumeText.text = Mathf.RoundToInt(soundVol * 100) + "%";
        }

        if (musicVolumeText != null)
        {
            float musicVol = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
            musicVolumeText.text = Mathf.RoundToInt(musicVol * 100) + "%";
        }

        if (brightnessText != null)
        {
            float brightness = PlayerPrefs.GetFloat("Brightness", defaultBrightness);
            brightnessText.text = Mathf.RoundToInt(brightness * 100) + "%";
        }
    }

    private void ApplyAllSettings()
    {
        float brightness = PlayerPrefs.GetFloat("Brightness", defaultBrightness);
        ApplyBrightness(brightness);
    }
}