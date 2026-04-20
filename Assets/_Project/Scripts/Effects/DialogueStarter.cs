using UnityEngine;

public class SceneDialogueStarter : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private int dialogueId = 15;
    [SerializeField] private string flagToSet = "talked_to_cat_loc2";
    [SerializeField] private bool autoStartDialogue = true;
    [SerializeField] private float startDelay = 0.5f; // Задержка перед началом

    private bool dialogueStarted = false;

    private void Start()
    {
        if (autoStartDialogue)
        {
            Invoke(nameof(StartIntroDialogue), startDelay);
        }
    }

    private void StartIntroDialogue()
    {
        if (dialogueStarted) return;
        dialogueStarted = true;

        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.Show(
                "",
                "Длинный коридор здания. Пол покрыт плиткой, местами на полу виднеются следы каких-то жидкостей. " +
                "На стенах стенды с объявлениями:\n" +
                "День открытых дверей, потерялась кошка...",
                StartMainDialogue
            );
        }
        else
        {
            StartMainDialogue();
        }
    }

    private void StartMainDialogue()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(dialogueId);
        }

        // Устанавливаем флаг
        if (FlagManager.Instance != null)
        {
            FlagManager.Instance.SetFlag(flagToSet, true);
            Debug.Log($"Установлен флаг: {flagToSet}");
        }
    }
}