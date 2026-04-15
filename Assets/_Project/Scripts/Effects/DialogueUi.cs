using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    private System.Action onContinue;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string speaker, string text, System.Action continueCallback)
    {
        dialoguePanel.SetActive(true);

        speakerText.text = speaker;
        dialogueText.text = text;

        onContinue = continueCallback;

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinuePressed);
    }

    private void OnContinuePressed()
    {
        onContinue?.Invoke();
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
    }
}