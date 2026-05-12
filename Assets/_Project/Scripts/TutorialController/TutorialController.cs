using System.Collections;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Dialogue IDs")]
    public int talkDialogueId = 2;

    private bool moved = false;
    private bool dialogueFinished = false;

    private int step = 0;

    private bool tutorialCompleted = false;

    IEnumerator Start()
    {
        // Ждём 1 кадр чтобы DialogueUI успел создаться
        yield return null;

        // Если обучение уже проходили
        if (PlayerPrefs.GetInt("tutorial_completed", 0) == 1)
        {
            Debug.Log("Tutorial already completed");

            gameObject.SetActive(false);
            yield break;
        }

        // Проверяем наличие систем
        if (DialogueUI.Instance == null)
        {
            Debug.LogError("DialogueUI.Instance == NULL");
            yield break;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager.Instance == NULL");
            yield break;
        }

        StartMoveStep();
    }

    void Update()
    {
        if (tutorialCompleted)
            return;

        // SKIP
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Tutorial skipped");

            CompleteTutorial();
            return;
        }

        // ШАГ 1 -> ШАГ 2
        if (step == 0 && moved)
        {
            StartDialogueStep();
        }

        // КОНЕЦ ОБУЧЕНИЯ
        if (step == 1 && dialogueFinished)
        {
            CompleteTutorial();
        }
    }

    // ==================================================
    // ШАГ 1 — ДВИЖЕНИЕ
    // ==================================================

    void StartMoveStep()
    {
        Debug.Log("Tutorial Step 1: Move");

        step = 0;

        DialogueUI.Instance.Message(
            "",
            "Используй A/D или стрелки чтобы двигаться",
            null
        );
    }

    public void OnPlayerMoved()
    {
        if (step != 0)
            return;

        if (moved)
            return;

        moved = true;

        Debug.Log("Player moved");
    }

    // ==================================================
    // ШАГ 2 — ДИАЛОГ
    // ==================================================

    void StartDialogueStep()
    {
        Debug.Log("Tutorial Step 2: Dialogue");

        step = 1;

        dialogueFinished = false;

        DialogueManager.Instance.StartDialogue(
            talkDialogueId,
            OnDialogueFinished
        );
    }

    void OnDialogueFinished()
    {
        Debug.Log("Tutorial dialogue finished");

        dialogueFinished = true;
    }

    // ==================================================
    // ЗАВЕРШЕНИЕ
    // ==================================================

    void CompleteTutorial()
    {
        if (tutorialCompleted)
            return;

        tutorialCompleted = true;

        PlayerPrefs.SetInt("tutorial_completed", 1);
        PlayerPrefs.Save();

        Debug.Log("Tutorial completed");

        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.Hide();
        }

        gameObject.SetActive(false);
    }

    // ==================================================
    // ДЛЯ ТЕСТА
    // ==================================================

    [ContextMenu("Reset Tutorial")]
    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("tutorial_completed");

        Debug.Log("Tutorial reset");
    }
}