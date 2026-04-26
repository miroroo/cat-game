using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-90)]
public class FlagManager : MonoBehaviour
{
    // Singleton-экземпляр менеджера флагов
    public static FlagManager Instance { get; private set; }

    // Словарь для хранения флагов и числовых значений
    private Dictionary<string, int> flags = new Dictionary<string, int>();

    /// <summary>
    /// Создаёт Singleton-экземпляр и загружает флаги из базы данных.
    /// Удаляет дубликат объекта при повторном создании.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadFlagsFromDB();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Загружает все флаги из базы данных в словарь flags.
    /// </summary>
    private void LoadFlagsFromDB()
    {
        flags.Clear();
        var db = DatabaseManager.Instance.Connection;

        List<Flag> allFlags = db.Table<Flag>().ToList();

        foreach (var flag in allFlags)
        {
            flags[flag.flagName] = flag.flagValue;
        }

        Debug.Log($"Загружено {flags.Count} флагов");
    }

    /// <summary>
    /// Возвращает bool-значение флага:
    /// true если значение равно 1, иначе false.
    /// </summary>
    public bool GetFlag(string flagName)
    {
        return GetValue(flagName) == 1;
    }

    /// <summary>
    /// Устанавливает bool-значение флага,
    /// преобразуя true/false в 1/0.
    /// </summary>
    public void SetFlag(string flagName, bool value)
    {
        SetValue(flagName, value ? 1 : 0);
        Debug.Log($"SET FLAG: {flagName} = {value}");
    }

    /// <summary>
    /// Возвращает числовое значение флага.
    /// Если ключ не найден, возвращает 0.
    /// </summary>
    public int GetInt(string key)
    {
        return GetValue(key);
    }

    /// <summary>
    /// Устанавливает числовое значение флага
    /// и сохраняет его в базе данных.
    /// </summary>
    public void SetInt(string key, int value)
    {
        SetValue(key, value);
    }

    /// <summary>
    /// Универсальный метод получения значения из словаря.
    /// </summary>
    private int GetValue(string key)
    {
        return flags.TryGetValue(key, out int value) ? value : 0;
    }

    /// <summary>
    /// Универсальный метод сохранения значения
    /// в словарь и базу данных.
    /// </summary>
    private void SetValue(string key, int value)
    {
        flags[key] = value;

        var db = DatabaseManager.Instance.Connection;

        var existing = db.Table<Flag>()
            .FirstOrDefault(f => f.flagName == key);

        if (existing != null)
        {
            existing.flagValue = value;
            db.Update(existing);
        }
        else
        {
            db.Insert(new Flag
            {
                flagName = key,
                flagValue = value
            });
        }
    }

    /// <summary>
    /// Сбрасывает все флаги в значение 0
    /// в памяти и в базе данных.
    /// </summary>
    public void ResetAllFlags()
    {
        var db = DatabaseManager.Instance.Connection;

        foreach (var key in flags.Keys.ToList())
        {
            flags[key] = 0;
        }

        var allFlags = db.Table<Flag>().ToList();

        foreach (var flag in allFlags)
        {
            flag.flagValue = 0;
            db.Update(flag);
        }

        Debug.Log("Все флаги сброшены");
    }
}
