using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance { get; private set; }

    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

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
    private void LoadFlagsFromDB()
    {
        flags.Clear();
        var db = DatabaseManager.Instance.Connection;

        // Загружаем все флаги
        List<Flag> allFlags = db.Table<Flag>().ToList();

        foreach (var flag in allFlags)
        {
            flags[flag.flagName] = flag.flagValue;
        }

        Debug.Log($"Загружено {flags.Count} флагов");
    }

    public bool GetFlag(string flagName)
    {
        return flags.TryGetValue(flagName, out bool value) && value;
    }

    public void SetFlag(string flagName, bool value)
    {
        flags[flagName] = value;

        var db = DatabaseManager.Instance.Connection;

        // Проверяем, есть ли уже такой флаг в БД
        var existingFlag = db.Table<Flag>()
            .FirstOrDefault(f => f.flagName == flagName);

        if (existingFlag != null)
        {
            // Обновляем существующий
            existingFlag.flagValue = value;
            db.Update(existingFlag);
        }
        else
        {
            // Вставляем новый
            db.Insert(new Flag { flagName = flagName, flagValue = value });
        }


    }
}