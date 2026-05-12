using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    private float currentMusicVolume = 0.75f;
    private float currentSFXVolume = 0.75f;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке новых сцен
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        LoadVolumes();
        ApplyVolumes();
    }
    
    private void LoadVolumes()
    {
        currentSFXVolume = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
    }
    
    private void ApplyVolumes()
    {
        if (musicSource != null) musicSource.volume = currentMusicVolume;
        if (sfxSource != null) sfxSource.volume = currentSFXVolume;
    }
    
    public void SetMusicVolume(float volume)
    {
        currentMusicVolume = volume;
        if (musicSource != null) musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }
    
    public void SetSFXVolume(float volume)
    {
        currentSFXVolume = volume;
        if (sfxSource != null) sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SoundVolume", volume);
        PlayerPrefs.Save();
    }
    
    // Для проигрывания звуков кнопок
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}