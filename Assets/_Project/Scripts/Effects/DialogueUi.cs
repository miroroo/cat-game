using UnityEngine;
using TMPro;
using System.Collections;
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

    private float normalHeight;
    private float smallHeight;
    private RectTransform panelRect;

    private System.Action onContinue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(transform.root.gameObject);

            panelRect = dialoguePanel.GetComponent<RectTransform>();

            normalHeight = panelRect.sizeDelta.y;
            smallHeight = normalHeight / 2f;
        }
        else
        {
            Destroy(transform.root.gameObject);
            return;
        }

        Hide();

        if (blocker != null)
            blocker.SetActive(false);
    }

    public void Show(string speaker, string text, System.Action continueCallback)
    {
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x,normalHeight);

        leftCat.enabled = true;
        rightSasha.enabled = true;

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
        if (onContinue != null)
            onContinue.Invoke();
        else
            Hide();
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
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (blocker != null)
            blocker.SetActive(false);
    }



    public void Message(string speaker, string message, System.Action continueCallback)
    {
        dialoguePanel.SetActive(true);

        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x,smallHeight);


        leftCat.enabled = false;
        rightSasha.enabled = false;

        speakerText.text = speaker;
        dialogueText.text = message;

        dialogueText.textWrappingMode = TextWrappingModes.Normal;
        dialogueText.overflowMode = TextOverflowModes.Overflow;

        continueButton.gameObject.SetActive(true); 
        onContinue = continueCallback;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinuePressed);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}