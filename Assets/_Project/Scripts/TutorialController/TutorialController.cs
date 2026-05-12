using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public int moveDialogueId = 1;      // диалог "двигайся"
    public int talkDialogueId = 2;      // диалог "поговори"

    private bool moved = false;
    private bool dialogueFinished = false;
    private int step = 0;

    void Start()
    {
        // если обучение уже пройдено — ничего не делаем
        if (PlayerPrefs.GetInt("tutorial_completed", 0) == 1)
        {
            gameObject.SetActive(false);
            return;
        }

        StartMoveStep();
    }

    void Update()
    {
        // SKIP
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CompleteTutorial();
        }

        if (step == 0 && moved)
        {
            StartDialogueStep();
        }

        if (step == 1 && dialogueFinished)
        {
            CompleteTutorial();
        }
    }

    // === ШАГ 1: ДВИЖЕНИЕ ===
    void StartMoveStep()
    {
        step = 0;

        DialogueUI.Instance.Message(
            "",
            "Используй A/D или стрелки чтобы двигаться",
            null
        );
    }

    public void OnPlayerMoved()
    {
        if (step == 0)
        {
            moved = true;
        }
    }

    // === ШАГ 2: ДИАЛОГ ===
    void StartDialogueStep()
    {
        step = 1;
        dialogueFinished = false;

        DialogueManager.Instance.StartDialogue(talkDialogueId, OnDialogueFinished);
    }

    void OnDialogueFinished()
    {
        dialogueFinished = true;
    }

    // === КОНЕЦ ===
    void CompleteTutorial()
    {
        PlayerPrefs.SetInt("tutorial_completed", 1);

        Debug.Log("Tutorial completed");

        gameObject.SetActive(false);
    }
}