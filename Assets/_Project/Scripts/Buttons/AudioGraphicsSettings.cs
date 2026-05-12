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
        LoadSettings();
        SetupUIListeners();
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
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSoundVolume(value);

        UpdateTexts();
    }

    private void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);

        UpdateTexts();
    }

    private void OnBrightnessChanged(float value)
    {
        if (BrightnessManager.Instance != null)
            BrightnessManager.Instance.SetBrightness(value);

        UpdateTexts();
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
        if (soundVolumeText != null && AudioManager.Instance != null)
            soundVolumeText.text = Mathf.RoundToInt(AudioManager.Instance.GetSoundVolume() * 100) + "%";

        if (musicVolumeText != null && AudioManager.Instance != null)
            musicVolumeText.text = Mathf.RoundToInt(AudioManager.Instance.GetMusicVolume() * 100) + "%";

        if (brightnessText != null && BrightnessManager.Instance != null)
        {
            brightnessText.text = Mathf.RoundToInt(BrightnessManager.Instance.GetBrightness() * 100) + "%";
        }
    }

    private void ApplyAllSettings()
    {
        float brightness = PlayerPrefs.GetFloat("Brightness", defaultBrightness);
        ApplyBrightness(brightness);
    }
}