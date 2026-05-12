using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour
{
    private bool moved = false;
    private int step = 0;

    void Start()
    {
        Debug.Log("=== TUTORIAL START ===");

        if (FlagManager.Instance.GetFlag("tutorial"))
        {
            enabled = false;
            return;
        }

        // Запускаем корутину, которая ждёт появления DialogueUI
        StartCoroutine(WaitForDialogueUIAndStart());
    }

    IEnumerator WaitForDialogueUIAndStart()
    {
        Debug.Log("Ожидание DialogueUI...");

        if (DialogueUI.Instance == null)
        {
            Debug.LogError("DialogueUI.Instance не появился! Проверьте, что на сцене есть объект с DialogueUI");
            yield break;
        }

        Debug.Log("DialogueUI.Instance найден!");

        // Небольшая задержка для уверенности
        yield return new WaitForSeconds(0.2f);

        StartMoveStep();
    }

    void Update()
    {
        // Пропуск туториала по Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CompleteTutorial();
        }
    }

    void StartMoveStep()
    {
        step = 0;
        moved = false;

        Debug.Log("StartMoveStep - показываем сообщение");

        if (DialogueUI.Instance == null)
        {
            Debug.LogError("DialogueUI.Instance = NULL!");
            return;
        }

        // Показываем сообщение
        DialogueUI.Instance.Message(
            "Обучение",
            "Используй клавиши A/D или стрелки ← → чтобы двигаться",
            null
        );

        Debug.Log("Сообщение отправлено");
    }

    public void OnPlayerMoved()
    {
        Debug.Log($"OnPlayerMoved вызван! step={step}, moved={moved}");

        if (step == 0 && !moved)
        {
            moved = true;
            Debug.Log("Игрок пошевелился - запускаем диалог");
            StartDialogueStep();
        }
    }

    void StartDialogueStep()
    {
        step = 1;
        Debug.Log("StartDialogueStep - запуск диалога");

        if (DialogueUI.Instance == null)
        {
            Debug.LogError("DialogueUI.Instance не найден!");
            CompleteTutorial();
            return;
        }

        // Закрываем предыдущее сообщение если открыто
        DialogueUI.Instance.Hide();

        // Запускаем полноценный диалог
        DialogueUI.Instance.Show(
            "Саша",
            "Отлично! Ты научился двигаться! Теперь научимся переключать диалог\nНажки пробел, чтобы увидеть следующую фразу.",
            OnDialogueFinished
        );
    }

    void OnDialogueFinished()
    {
        Debug.Log("Диалог завершён");
        DialogueUI.Instance.Show(
            "Саша",
            "Чтобы взаимодействовать с предметами нажми на них!\n" +
            "Нажми пкм на окно.",
            CompleteTutorial
        );
    }

    void CompleteTutorial()
    {
        Debug.Log("Туториал завершен!");
        FlagManager.Instance.SetFlag("tutorial", true);

        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.Hide();
        }

        enabled = false;
    }
}