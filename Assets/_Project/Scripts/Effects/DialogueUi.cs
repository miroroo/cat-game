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

    private float normalHeight;
    private float smallHeight;
    private RectTransform panelRect;
    private string fullText;              // полный текст текущей реплики
    private int currentPage = 0;          // текущая страница
    private int totalPages = 1;           // всего страниц
    private const int CHARS_PER_PAGE = 250;
    private System.Action onContinue;
    private System.Action onPageComplete; // новое: вызывается когда все страницы показаны

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
    // Показывает текущую страницу текста
    private void ShowCurrentPage()
    {
        int startIndex = currentPage * CHARS_PER_PAGE;

        // Если это последняя страница или текст короче одной страницы
        if (currentPage >= totalPages - 1)
        {
            // Показываем остаток текста
            dialogueText.text = fullText.Substring(startIndex);

            // Меняем текст кнопки на "Продолжить" (переход к следующей реплике)
            if (continueButton != null)
            {
                var buttonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null) buttonText.text = "Продолжить →";
            }
        }
        else
        {
            // Показываем часть текста
            int length = Mathf.Min(CHARS_PER_PAGE, fullText.Length - startIndex);
            dialogueText.text = fullText.Substring(startIndex, length) + "...";

            // Меняем текст кнопки на "Далее" (следующая страница)
            if (continueButton != null)
            {
                var buttonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null) buttonText.text = "Далее ▼";
            }
        }
    }

    // Переопределяем обработку продолжения
    private void OnContinuePressed()
    {
        musicSource.PlayOneShot(backgroundMusic, 0.3f);
        currentPage++;

        if (currentPage < totalPages)
        {
            // Ещё есть страницы этой же реплики — показываем следующую
            ShowCurrentPage();
        }
        else
        {
            // Все страницы показаны — переходим к следующей реплике
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

    public void Show(string speaker, string text, System.Action continueCallback)
    {
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, normalHeight);

        leftCat.enabled = true;
        rightSasha.enabled = true;

        dialoguePanel.SetActive(true);
        blocker.SetActive(true);

        UpdateCharacters(speaker);

        speakerText.text = speaker;

        // СОХРАНЯЕМ ПОЛНЫЙ ТЕКСТ
        fullText = text;
        currentPage = 0;

        // ВЫЧИСЛЯЕМ КОЛИЧЕСТВО СТРАНИЦ
        totalPages = Mathf.CeilToInt((float)fullText.Length / CHARS_PER_PAGE);

        // ПОКАЗЫВАЕМ ПЕРВУЮ СТРАНИЦУ
        ShowCurrentPage();

        dialogueText.textWrappingMode = TextWrappingModes.Normal;
        dialogueText.overflowMode = TextOverflowModes.Overflow;

        continueButton.gameObject.SetActive(true);

        onContinue = continueCallback;
        onPageComplete = null; // сбрасываем

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



    void Update()
    {
        if (dialoguePanel.activeSelf)
        {
            bool shouldContinue = false;

            // Клавиши
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                shouldContinue = true;
            }

            // Клик мыши по панели диалога (правильный способ для UI)
            if (Input.GetMouseButtonDown(0))
            {
                // Создаём PointerEventData для проверки
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;

                // Список объектов под мышью
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                // Проверяем, есть ли среди них наша панель
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

        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x,smallHeight);


        leftCat.enabled = false;
        rightSasha.enabled = false;

        speakerText.text = "";
        dialogueText.text = message;

        dialogueText.textWrappingMode = TextWrappingModes.Normal;
        dialogueText.overflowMode = TextOverflowModes.Overflow;

        continueButton.gameObject.SetActive(true); 
        onContinue = continueCallback;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinuePressed);
        // автоматически закрываем через 3 секунды
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