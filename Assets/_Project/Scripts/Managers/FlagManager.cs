using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-90)]
public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance { get; private set; }

    private Dictionary<string, int> flags = new Dictionary<string, int>();

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

        List<Flag> allFlags = db.Table<Flag>().ToList();

        foreach (var flag in allFlags)
        {
            flags[flag.flagName] = flag.flagValue; // теперь просто int
        }

        Debug.Log($"Загружено {flags.Count} флагов");
    }

    public bool GetFlag(string flagName)
    {

        return flags.TryGetValue(flagName, out int value) && value == 1;
    }

    public void SetFlag(string flagName, bool value)
    {
        int intValue = value ? 1 : 0;

        flags[flagName] = intValue;

        var db = DatabaseManager.Instance.Connection;

        var existingFlag = db.Table<Flag>()
            .FirstOrDefault(f => f.flagName == flagName);
        Debug.Log($"SET FLAG: {flagName} = {value}");

        if (existingFlag != null)
        {
            existingFlag.flagValue = intValue;
            db.Update(existingFlag);
        }
        else
        {
            db.Insert(new Flag
            {
                flagName = flagName,
                flagValue = intValue
            });
        }
    }

    public int GetInt(string key)
    {
        return flags.TryGetValue(key, out int value) ? value : 0;
    }

    public void SetInt(string key, int value)
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
            db.Insert(new Flag { flagName = key, flagValue = value });
        }
    }

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


//объединить методы