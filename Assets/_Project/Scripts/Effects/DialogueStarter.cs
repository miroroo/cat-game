using UnityEngine;

public class SceneDialogueStarter : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private int dialogueId = 32;
    [SerializeField] private string description = "";
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
            DialogueUI.Instance.Show("", description, StartMainDialogue);
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
    }
}