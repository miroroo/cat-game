using UnityEngine;
using SQLite;

[DefaultExecutionOrder(-100)]
public class DatabaseManager : MonoBehaviour
{
    // Singleton-экземпляр менеджера базы данных
    public static DatabaseManager Instance { get; private set; }
    private static bool _debug = true;
    // Соединение с SQLite-базой данных
    private SQLiteConnection _db;

    /// <summary>
    /// Создаёт Singleton-экземпляр и запускает инициализацию базы данных.
    /// Удаляет дубликат объекта, если экземпляр уже существует.
    /// </summary>
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

    /// <summary>
    /// Копирует базу данных в persistentDataPath и открывает соединение с SQLite.
    /// При ошибке выводит сообщение в консоль.
    /// </summary>
    private void InitializeDatabase()
    {
        try
        {
            string sourcePath = System.IO.Path.Combine(
                Application.streamingAssetsPath,
                "game_database.db"
            );

            string targetPath = System.IO.Path.Combine(
                Application.persistentDataPath,
                "game_database.db"
            );

            if (_debug)
            {
                if (System.IO.File.Exists(targetPath))
                {
                    System.IO.File.Delete(targetPath);
                    Debug.Log("Старая база удалена");
                }

                System.IO.File.Copy(sourcePath, targetPath);
                Debug.Log("Новая база скопирована в persistentDataPath");
            }
            else
            {
                if (!System.IO.File.Exists(targetPath))
                {
                    System.IO.File.Copy(sourcePath, targetPath);
                    Debug.Log("База впервые скопирована в persistentDataPath");
                }
            }

            Debug.Log($"Подключаемся к БД: {targetPath}");

            _db = new SQLiteConnection(
                targetPath,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create
            );

            Debug.Log("База данных успешно открыта");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Не удалось открыть БД: {ex.Message}");
            _db = null;
        }
    }

    /// <summary>
    /// Возвращает активное соединение с базой данных.
    /// Если соединение отсутствует, выводит ошибку.
    /// </summary>
    public SQLiteConnection Connection
    {
        get
        {
            if (_db == null)
                Debug.LogError("Соединение с БД не установлено!");

            return _db;
        }
    }

    /// <summary>
    /// Закрывает соединение с базой данных и освобождает ресурсы.
    /// Вызывается при уничтожении объекта.
    /// </summary>
    private void OnDestroy()
    {
        if (_db != null)
        {
            Debug.Log("DatabaseManager уничтожен");

            _db.Close();
            _db.Dispose();
            _db = null;
        }

        if (Instance == this)
            Instance = null;
    }
}