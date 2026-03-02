using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    //public bool hasStartedQuest = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ItemDatabase.LoadAllItems();
    }

    void Start()
    {
    }

    // Здесь можно добавить методы для сохранения/загрузки, управления сценами и т.д.
}