using UnityEngine;
using UnityEngine.SceneManagement;

public class Phone : InteractableObject
{
    [Header("Dialogue Flag")]
    [SerializeField] private string requiredFlag1 = "1_call";
    [SerializeField] private string requiredFlag2 = "2_call_permit";

    public override void Interact()
    {
        base.Interact();
        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог идёт — нельзя взаимодействовать");
            return;
        }

        bool firstCall = FlagManager.Instance.GetFlag(requiredFlag1);
        Debug.Log($"Флаг {requiredFlag1} = {firstCall}");

        // Первый звонок
        if (!firstCall)
        {

            FlagManager.Instance.SetFlag(requiredFlag1, true);
            Debug.Log("*** тишина ***");
            DialogueUI.Instance?.Message(
                "",
                "*** тишина ***",
                () => Invoke(nameof(StartFirstCallDialogue), 2.5f)
            );

            return;
        }

        bool secondCall = FlagManager.Instance.GetFlag(requiredFlag2);
        Debug.Log($"Флаг {requiredFlag2} = {secondCall}");

        // Если есть разрешение на второй звонок
        if (secondCall)
        {
            DialogueUI.Instance?.Message(
                "",
                "<color=red>*** UNKNOWN ERROR ***  *** UNKNOWN ERROR ***  *** UNKNOWN ERROR ***</color>",
                () => Invoke(nameof(StartSecondCallDialogue), 2.5f)
            );

            return;
        }
    }

    private void StartFirstCallDialogue()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(54);
        }
    }

    private void StartSecondCallDialogue()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(56);
        }
    }
}