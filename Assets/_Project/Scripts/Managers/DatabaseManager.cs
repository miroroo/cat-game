using UnityEngine;
using SQLite;

[DefaultExecutionOrder(-100)]
public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private SQLiteConnection _db;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDatabase()
    {
        try
        {
            string sourcePath = System.IO.Path.Combine(Application.streamingAssetsPath, "game_database.db");
            string targetPath = System.IO.Path.Combine(Application.persistentDataPath, "game_database.db");

            if (!System.IO.File.Exists(targetPath))
            {
                System.IO.File.Copy(sourcePath, targetPath);
                Debug.Log("База скопирована в persistentDataPath");
            }

            Debug.Log($"Подключаемся к БД: {targetPath}");

            _db = new SQLiteConnection(targetPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

            Debug.Log("База данных успешно открыта");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Не удалось открыть БД: {ex.Message}");
            _db = null;
        }
    }

    public SQLiteConnection Connection
    {
        get
        {
            if (_db == null)
                Debug.LogError("Соединение с БД не установлено!");
            return _db;
        }
    }


    // Закрываем соединение при выгрузке (на всякий случай)
    private void OnDestroy()
    {
        if (_db != null)
        {
            Debug.Log("DatabaseManager уничтожен");
            _db.Close();
            _db.Dispose();
        }
    }
}