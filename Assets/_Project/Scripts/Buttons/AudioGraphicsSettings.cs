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

	private float currentSoundVolume;
	private float currentMusicVolume;
	private float currentBrightness;

	private void Start()
	{
		LoadSettings();
		SetupUIListeners();
		ApplyAllSettings();
	}

	// Загрузка сохранённых настроек
	private void LoadSettings()
	{
		currentSoundVolume = PlayerPrefs.GetFloat("SoundVolume", defaultSoundVolume);
		currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
		currentBrightness = PlayerPrefs.GetFloat("Brightness", defaultBrightness);

		if (soundVolumeSlider != null) soundVolumeSlider.value = currentSoundVolume;
		if (musicVolumeSlider != null) musicVolumeSlider.value = currentMusicVolume;
		if (brightnessSlider != null) brightnessSlider.value = currentBrightness;

		UpdateTexts();
	}

	// Сохранение настроек
	private void SaveSettings()
	{
		PlayerPrefs.SetFloat("SoundVolume", currentSoundVolume);
		PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
		PlayerPrefs.SetFloat("Brightness", currentBrightness);
		PlayerPrefs.Save();
	}

	// Настройка обработчиков кнопок
	private void SetupUIListeners()
	{
		if (soundVolumeSlider != null)
			soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);

		if (musicVolumeSlider != null)
			musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

		if (brightnessSlider != null)
			brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
	}

	// Обработчики изменения слайдеров
	private void OnSoundVolumeChanged(float value)
	{
		currentSoundVolume = value;
		ApplySoundVolume();
		UpdateTexts();
		SaveSettings();
	}

	private void OnMusicVolumeChanged(float value)
	{
		currentMusicVolume = value;
		ApplyMusicVolume();
		UpdateTexts();
		SaveSettings();
	}

	private void OnBrightnessChanged(float value)
	{
		currentBrightness = value;
		ApplyBrightness();
		UpdateTexts();
		SaveSettings();
	}

	// Применение громкости звуков
	private void ApplySoundVolume()
	{
		AudioSource[] allSounds = FindObjectsOfType<AudioSource>();
		foreach (AudioSource source in allSounds)
		{
			if (source.CompareTag("SoundEffect"))
			{
				source.volume = currentSoundVolume;
			}
		}
	}

	// Применение громкости музыки
	private void ApplyMusicVolume()
	{
		AudioSource[] allSounds = FindObjectsOfType<AudioSource>();
		foreach (AudioSource source in allSounds)
		{
			if (source.CompareTag("Music"))
			{
				source.volume = currentMusicVolume;
			}
		}
	}

	// Применение яркости 
	private void ApplyBrightness()
	{
		Camera mainCamera = Camera.main;
		if (mainCamera != null)
		{
			// Изменяем цвет фона камеры
			float brightness = currentBrightness;
			mainCamera.backgroundColor = new Color(brightness, brightness, brightness, 1f);
		}
	}

	// Обновление текстов с процентами
	private void UpdateTexts()
	{
		if (soundVolumeText != null)
			soundVolumeText.text = Mathf.RoundToInt(currentSoundVolume * 100) + "%";

		if (musicVolumeText != null)
			musicVolumeText.text = Mathf.RoundToInt(currentMusicVolume * 100) + "%";

		if (brightnessText != null)
			brightnessText.text = Mathf.RoundToInt(currentBrightness * 100) + "%";
	}

	// Применяем все настройки сразу
	private void ApplyAllSettings()
	{
		ApplySoundVolume();
		ApplyMusicVolume();
		ApplyBrightness();
	}

	// Публичные методы для получения настроек из других скриптов
	public float GetSoundVolume() => currentSoundVolume;
	public float GetMusicVolume() => currentMusicVolume;
	public float GetBrightness() => currentBrightness;
}