using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [Header("Sizes")]
    [SerializeField] private float panelHeightNormal = 170f;
    [SerializeField] private float panelHeightSmall = 150f;
    [SerializeField] private float imageSize = 350f; // Размер изображений

    private RectTransform panelRect;
    private RectTransform leftCatRect;
    private RectTransform rightSashaRect;

    private string fullText;
    private int currentPage = 0;
    private int totalPages = 1;
    private const int CHARS_PER_PAGE = 250;
    private System.Action onContinue;

    private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;

    private void Start()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        musicSource.playOnAwake = false;
        musicSource.clip = backgroundMusic;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);

            panelRect = dialoguePanel.GetComponent<RectTransform>();

            // Получаем RectTransform для изображений
            if (leftCat != null)
                leftCatRect = leftCat.GetComponent<RectTransform>();
            if (rightSasha != null)
                rightSashaRect = rightSasha.GetComponent<RectTransform>();
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
        // Восстанавливаем нормальную высоту
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, panelHeightNormal);

        // Показываем изображения
        if (leftCat != null)
        {
            leftCat.enabled = true;
            leftCat.gameObject.SetActive(true);
        }
        if (rightSasha != null)
        {
            rightSasha.enabled = true;
            rightSasha.gameObject.SetActive(true);
        }

        // Настраиваем размеры изображений
        SetImageSize();

        dialoguePanel.SetActive(true);
        if (blocker != null) blocker.SetActive(true);

        UpdateCharacters(speaker);

        if (speakerText != null)
            speakerText.text = speaker;

        fullText = text;
        currentPage = 0;
        totalPages = Mathf.CeilToInt((float)fullText.Length / CHARS_PER_PAGE);

        ShowCurrentPage();

        if (dialogueText != null)
        {
            dialogueText.textWrappingMode = TextWrappingModes.Normal;
            dialogueText.overflowMode = TextOverflowModes.Overflow;
        }

        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinuePressed);
        }

        onContinue = continueCallback;
    }

    private void SetImageSize()
    {
        // Устанавливаем фиксированный размер для изображений
        if (leftCatRect != null)
        {
            leftCatRect.sizeDelta = new Vector2(imageSize, imageSize);
        }
        if (rightSashaRect != null)
        {
            rightSashaRect.sizeDelta = new Vector2(imageSize, imageSize);
        }
    }

    private void ShowCurrentPage()
    {
        if (dialogueText == null) return;

        int startIndex = currentPage * CHARS_PER_PAGE;

        if (currentPage >= totalPages - 1)
        {
            dialogueText.text = fullText.Substring(startIndex);
        }
        else
        {
            int length = Mathf.Min(CHARS_PER_PAGE, fullText.Length - startIndex);
            dialogueText.text = fullText.Substring(startIndex, length) + "...";
        }
    }

    private void OnContinuePressed()
    {
        if (backgroundMusic != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(backgroundMusic, 0.3f);
        }

        currentPage++;

        if (currentPage < totalPages)
        {
            ShowCurrentPage();
        }
        else
        {
            if (onContinue != null)
            {
                onContinue.Invoke();
            }
            else
            {
                Hide();
            }
        }
    }

    void UpdateCharacters(string speaker)
    {
        if (leftCat == null || rightSasha == null) return;

        Color active = Color.white;
        Color darker = new Color(0.5f, 0.5f, 0.5f, 1f);

        if (speaker == "Саша")
        {
            if (leftCat != null) leftCat.color = darker;
            if (rightSasha != null) rightSasha.color = active;
        }
        else if (speaker == "Марсик")
        {
            if (leftCat != null) leftCat.color = active;
            if (rightSasha != null) rightSasha.color = darker;
        }
        else
        {
            if (leftCat != null) leftCat.color = darker;
            if (rightSasha != null) rightSasha.color = darker;
        }
    }

    void Update()
    {
        if (dialoguePanel != null && dialoguePanel.activeSelf)
        {
            bool shouldContinue = false;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                shouldContinue = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject == dialoguePanel)
                    {
                        shouldContinue = true;
                        break;
                    }
                }
            }

            if (shouldContinue)
            {
                OnContinuePressed();
            }
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
        if (blocker != null) blocker.SetActive(true);

        // Устанавливаем маленькую высоту для сообщений
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, panelHeightSmall);

        // Скрываем изображения для сообщений
        if (leftCat != null) leftCat.enabled = false;
        if (rightSasha != null) rightSasha.enabled = false;

        if (speakerText != null) speakerText.text = "";
        if (dialogueText != null)
        {
            dialogueText.text = message;
            dialogueText.textWrappingMode = TextWrappingModes.Normal;
            dialogueText.overflowMode = TextOverflowModes.Overflow;
        }

        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinuePressed);
        }

        onContinue = continueCallback;
        StartCoroutine(AutoCloseMessage());
    }

    private IEnumerator AutoCloseMessage()
    {
        yield return new WaitForSeconds(2f);
        Hide();
        if (onContinue != null)
        {
            onContinue.Invoke();
        }
    }
}