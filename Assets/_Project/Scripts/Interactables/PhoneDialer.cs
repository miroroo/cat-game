using UnityEngine;
using TMPro;

public class PhoneDialer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private GameObject phonePanel;

    [Header("Story Numbers")]
    [SerializeField] private string firstStoryNumber = "111";
    [SerializeField] private string secondStoryNumber = "222";

    [Header("Dialogue Flags")]
    [SerializeField] private string firstCallFlag = "1_call";
    [SerializeField] private string secondCallPermitFlag = "2_call_permit";

    private string currentNumber = "";

    public void PressDigit(string digit)
    {
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
    }

    public void Call()
    {
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог уже идёт — звонить нельзя");
            return;
        }

        if (currentNumber == firstStoryNumber)
        {
            FirstCall();
            return;
        }

        if (currentNumber == secondStoryNumber)
        {
            SecondCall();
            return;
        }

        WrongNumber();
    }

    private void FirstCall()
    {
        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        bool firstCallDone = FlagManager.Instance.GetFlag(firstCallFlag);

        if (firstCallDone)
        {
            DialogueUI.Instance?.Message("", "*** никто не отвечает ***", null);
            return;
        }

        FlagManager.Instance.SetFlag(firstCallFlag, true);

        DialogueUI.Instance?.Message(
            "",
            "*** тишина ***",
            () => Invoke(nameof(StartFirstCallDialogue), 2.5f)
        );
    }

    private void SecondCall()
    {
        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        bool secondCallAllowed = FlagManager.Instance.GetFlag(secondCallPermitFlag);

        if (!secondCallAllowed)
        {
            DialogueUI.Instance?.Message("", "*** номер недоступен ***", null);
            return;
        }

        DialogueUI.Instance?.Message(
            "",
            "<color=red>*** UNKNOWN ERROR ***  *** UNKNOWN ERROR ***  *** UNKNOWN ERROR ***</color>",
            () => Invoke(nameof(StartSecondCallDialogue), 2.5f)
        );
    }

    private void WrongNumber()
    {
        DialogueUI.Instance?.Message("", "*** номер не отвечает ***", null);
    }

    private void StartFirstCallDialogue()
    {
        DialogueManager.Instance?.StartDialogue(54);
    }

    private void StartSecondCallDialogue()
    {
        DialogueManager.Instance?.StartDialogue(56, LoadNextScene);
    }

    private void LoadNextScene()
    {
        SceneLoader.Instance.LoadLocation("Fiziks");
    }

    private void UpdateDisplay()
    {
        if (displayText != null)
            displayText.text = currentNumber;
    }
}