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
            ReloadDatabase();
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
            string sourcePath = System.IO.Path.Combine(Application.streamingAssetsPath,"game_database.db");

            string targetPath = System.IO.Path.Combine(Application.persistentDataPath,"game_database.db");

            if (!System.IO.File.Exists(targetPath))
            {
                System.IO.File.Copy(sourcePath, targetPath);
            }

            Debug.Log("Новая база скопирована в persistentDataPath");

            Debug.Log($"Подключаемся к БД: {targetPath}");

            _db = new SQLiteConnection(targetPath,SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

            Debug.Log("База данных успешно открыта");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Не удалось открыть БД: {ex.Message}");
            _db = null;
        }
    }

    public void ReloadDatabase()
    {
        try
        {
            // Сначала закрываем существующее соединение
            if (_db != null)
            {
                Debug.Log("Закрываем текущее соединение с БД");
                _db.Close();
                _db.Dispose();
                _db = null;
            }

            // Небольшая задержка чтобы ОС освободила файл
            System.Threading.Thread.Sleep(100);

            string sourcePath = System.IO.Path.Combine(Application.streamingAssetsPath, "game_database.db");
            string targetPath = System.IO.Path.Combine(Application.persistentDataPath, "game_database.db");

            // Удаляем старый файл
            if (System.IO.File.Exists(targetPath))
            {
                System.IO.File.Delete(targetPath);
                Debug.Log("Старая база удалена");
            }

            // Копируем новый
            System.IO.File.Copy(sourcePath, targetPath);
            Debug.Log("Новая база скопирована в persistentDataPath");

            // Открываем новое соединение
            _db = new SQLiteConnection(targetPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            Debug.Log("База данных успешно переоткрыта");

            // Оповещаем DialogueManager о перезагрузке
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.ForceReloadDialogues();
            }

            // Также оповести другие системы, которые кешируют данные из БД
            if (FlagManager.Instance != null)
            {
                FlagManager.Instance.ResetAllFlags(); // если есть такой метод
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Не удалось переоткрыть БД: {ex.Message}");
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