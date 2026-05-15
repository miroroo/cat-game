using TMPro;
using UnityEngine;

public class PhoneDialer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private GameObject phonePanel;
    public GameObject blocker;
    [SerializeField] private Collider2D exitCollider;

    [Header("Audio")]
    [SerializeField] private AudioClip callSound;
    [SerializeField] private AudioClip buttonPressSound;

    [Header("Dialogue Flags")]
    [SerializeField] private string firstCallFlag = "1_call";
    [SerializeField] private string secondCallPermitFlag = "2_call_permit";
    [SerializeField] private string secondCallFlag = "2_call";

    [Header("Dialogues")]
    [SerializeField] private int firstDialogueID = 54;
    [SerializeField] private int secondDialogueID = 56;

    private string currentNumber = "";
    private int callCount = 0;

    public void PressDigit(string digit)
    {
        // Звук нажатия кнопки
        PlayButtonSound();

        currentNumber += digit;
        UpdateDisplay();
    }

    public void ClearNumber()
    {
        currentNumber = "";
        UpdateDisplay();
    }

    public void ClosePhone()
    {
        ClearNumber();

        if (phonePanel != null)
            phonePanel.SetActive(false);
        if (blocker != null)
            blocker.SetActive(false);
        if (exitCollider != null)
            exitCollider.enabled = true;
    }

    public void Call()
    {
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог уже идёт — звонить нельзя");
            return;
        }

        // Звук звонка
        PlayCallSound();

        callCount++;

        switch (callCount)
        {
            case 1:
                FirstCall();
                break;
            case 2:
                SecondCall();
                break;
            default:
                SubsequentCall();
                break;
        }
    }

    private void PlayButtonSound()
    {
        if (buttonPressSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(buttonPressSound);
        }
    }

    private void PlayCallSound()
    {
        if (callSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(callSound);
        }
    }

    private void FirstCall()
    {
        FlagManager.Instance?.SetFlag(firstCallFlag, true);

        DialogueUI.Instance?.Message(
            "",
            "*** тишина ***",
            () => Invoke(nameof(StartFirstCallDialogue), 0.2f)
        );
    }

    private void SecondCall()
    {
        FlagManager.Instance?.SetFlag(secondCallFlag, true);

        DialogueUI.Instance?.Message(
            "",
            "<color=red>*** UNKNOWN ERROR ***  *** UNKNOWN ERROR ***  *** UNKNOWN ERROR ***</color>",
            () => Invoke(nameof(StartSecondCallDialogue), 0.6f)
        );
    }

    private void SubsequentCall()
    {
        DialogueUI.Instance?.Message("", "*** скажи 300 ***", null);
    }

    private void StartFirstCallDialogue()
    {
        DialogueManager.Instance?.StartDialogue(firstDialogueID);
    }

    private void StartSecondCallDialogue()
    {
        DialogueManager.Instance?.StartDialogue(secondDialogueID);
    }

    private void UpdateDisplay()
    {
        if (displayText != null)
            displayText.text = currentNumber;
    }
}