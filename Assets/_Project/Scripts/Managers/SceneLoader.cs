using System.Linq;
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
    /// Получить last_scene из таблицы Scenes
    /// </summary>
    public string GetLastScene()
    {
        if (DatabaseManager.Instance?.Connection == null)
        {
            Debug.LogError("DatabaseManager не инициализирован");
            return "";
        }

        var db = DatabaseManager.Instance.Connection;

        var sceneRecord = db.Table<Scenes>()
            .FirstOrDefault(x => x.id == 1);

        if (sceneRecord == null)
        {
            Debug.LogWarning("Запись Scenes с id = 1 не найдена");
            return "";
        }

        Debug.Log("Получена сцена из БД: " + sceneRecord.last_scene);
        return sceneRecord.last_scene;
    }

    /// <summary>
    /// Сохранить last_scene в таблицу Scenes
    /// </summary>
    public void SetLastScene(string sceneName)
    {
        if (DatabaseManager.Instance?.Connection == null)
        {
            Debug.LogError("DatabaseManager не инициализирован");
            return;
        }

        var db = DatabaseManager.Instance.Connection;

        var sceneRecord = db.Table<Scenes>()
            .FirstOrDefault(x => x.id == 1);

        if (sceneRecord == null)
        {
            sceneRecord = new Scenes
            {
                id = 1,
                last_scene = sceneName
            };

            db.Insert(sceneRecord);
        }
        else
        {
            sceneRecord.last_scene = sceneName;
            db.Update(sceneRecord);
        }

        Debug.Log("Сцена сохранена в БД: " + sceneName);
    }

    /// <summary>
    /// Сохранить сцену в БД и загрузить её
    /// </summary>
    public void LoadLocation(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Имя сцены пустое");
            return;
        }

        SetLastScene(sceneName);

        Debug.Log("Загрузка сцены: " + sceneName);

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}