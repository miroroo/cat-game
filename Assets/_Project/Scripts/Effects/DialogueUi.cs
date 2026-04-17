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
    public Image leftCat;
    public Image rightSasha;
    public GameObject blocker;

    private System.Action onContinue;

    private void Awake()
    {
        Instance = this;
        Hide();
        blocker.SetActive(false);
    }

    public void Show(string speaker, string text, System.Action continueCallback)
    {
        dialoguePanel.SetActive(true);
        blocker.SetActive(true); // при диалоге

        UpdateCharacters(speaker);

        speakerText.text = speaker;
        dialogueText.text = text;

        dialogueText.textWrappingMode = TextWrappingModes.Normal;
        dialogueText.overflowMode = TextOverflowModes.Overflow;

        Debug.Log("SHOW UI: " + text);
        Debug.Log("Panel active: " + dialoguePanel.activeSelf);

        continueButton.gameObject.SetActive(true);

        onContinue = continueCallback;

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinuePressed);
    }

    void UpdateCharacters(string speaker)
    {
        Color active = Color.white; // обычный цвет
        Color darker = new Color(0.5f, 0.5f, 0.5f, 1f); // затемнённый (серый), непрозрачный

        if (speaker == "Саша")
        {
            leftCat.color = darker;
            rightSasha.color = active;
        }
        else if (speaker == "Марсик")
        {
            leftCat.color = active;
            rightSasha.color = darker;
        }
    }

    private void OnContinuePressed()
    {
        onContinue?.Invoke();
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            OnContinuePressed();
        }
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
        blocker.SetActive(false); // после
    }
}