using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Загружает локацию по имени сцены и обновляет текущую локацию в БД.
    /// </summary>
    public void LoadLocation(string sceneName)
    {
        // Сохраняем текущую локацию (если нужно) перед уходом
        SaveCurrentLocation(sceneName);

        // Загружаем сцену
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Загружает мини-игру (аддитивно, поверх текущей сцены).
    /// </summary>
    public void LoadMiniGame(string miniGameSceneName)
    {
        SceneManager.LoadScene(miniGameSceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Выгружает мини-игру (аддитивно загруженную).
    /// </summary>
    public void UnloadMiniGame(string miniGameSceneName)
    {
        SceneManager.UnloadSceneAsync(miniGameSceneName);
    }

    private void SaveCurrentLocation(string newLocation)
    {
        // Используем DatabaseManager для обновления таблицы PlayerProgress
        var db = DatabaseManager.Instance.Connection;
        var progress = db.Table<PlayerProgress>().FirstOrDefault();
        if (progress == null)
        {
            progress = new PlayerProgress { currentLocation = newLocation };
            db.Insert(progress);
        }
        else
        {
            progress.currentLocation = newLocation;
            db.Update(progress);
        }
    }
}