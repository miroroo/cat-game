using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Управляет отображением диалогового окна:
/// показывает реплики, переключает страницы текста и скрывает UI.
/// </summary>
public class DialogueUI : MonoBehaviour
{
    /// <summary>
    /// Singleton-экземпляр для глобального доступа к UI диалогов.
    /// </summary>
    public static DialogueUI Instance;

    [Header("UI")]
    public GameObject dialoguePanel;          // Основная панель диалога
    public TextMeshProUGUI speakerText;       // Имя говорящего персонажа
    public TextMeshProUGUI dialogueText;      // Текст реплики
    public Button continueButton;             // Кнопка продолжения
    public Image leftCat;                     // Спрайт кота слева
    public Image rightSasha;                  // Спрайт Саши справа
    public GameObject blocker;                // Блокирует взаимодействие с фоном

    private float normalHeight;               // Обычная высота панели
    private float smallHeight;                // Уменьшенная высота для коротких сообщений
    private RectTransform panelRect;          // RectTransform панели

    private string fullText;                  // Полный текст текущей реплики
    private int currentPage = 0;              // Текущая страница текста
    private int totalPages = 1;               // Общее количество страниц

    private const int CHARS_PER_PAGE = 250;   // Максимум символов на одной странице

    private System.Action onContinue;         // Действие после завершения реплики
    private System.Action onPageComplete;     // Действие после показа всех страниц

    private AudioSource musicSource;          // Источник звука
    [SerializeField] private AudioClip backgroundMusic; // Звук переключения реплик

    /// <summary>
    /// Инициализация аудиоисточника.
    /// </summary>
    private void Start()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.playOnAwake = false;
        musicSource.clip = backgroundMusic;
    }

    /// <summary>
    /// Singleton-инициализация и подготовка UI.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Сохраняем UI между сценами
            DontDestroyOnLoad(transform.root.gameObject);

            panelRect = dialoguePanel.GetComponent<RectTransform>();

            normalHeight = panelRect.sizeDelta.y;
            smallHeight = normalHeight / 2f;
        }
        else
        {
            // Удаляем дубликат UI
            Destroy(transform.root.gameObject);
            return;
        }

        Hide();

        // Отключаем блокировщик при старте
        if (blocker != null)
            blocker.SetActive(false);
    }

    /// <summary>
    /// Показывает текущую страницу длинной реплики.
    /// Если текст длинный — разбивает его на части.
    /// </summary>
    private void ShowCurrentPage()
    {
        int startIndex = currentPage * CHARS_PER_PAGE;

        // Последняя страница — показываем остаток текста
        if (currentPage >= totalPages - 1)
        {
            dialogueText.text = fullText.Substring(startIndex);
        }
        else
        {
            // Промежуточная страница — показываем часть текста
            int length = Mathf.Min(CHARS_PER_PAGE, fullText.Length - startIndex);
            dialogueText.text = fullText.Substring(startIndex, length) + "...";
        }
    }

    /// <summary>
    /// Обработка нажатия кнопки продолжения.
    /// Либо переключает страницу, либо запускает следующую реплику.
    /// </summary>
    private void OnContinuePressed()
    {
        musicSource.PlayOneShot(backgroundMusic, 0.3f);

        currentPage++;

        if (currentPage < totalPages)
        {
            // Есть ещё страницы текущей реплики
            ShowCurrentPage();
        }
        else
        {
            // Реплика закончилась
            if (onContinue != null)
                onContinue.Invoke();
            else
                Hide();
        }
    }

    /// <summary>
    /// Показывает полноценный диалог с персонажем.
    /// </summary>
    public void Show(string speaker, string text, System.Action continueCallback)
    {
        // Возвращаем обычную высоту панели
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, normalHeight);

        leftCat.enabled = true;
        rightSasha.enabled = true;

        dialoguePanel.SetActive(true);
        blocker.SetActive(true);

        UpdateCharacters(speaker);

        speakerText.text = speaker;

        // Сохраняем полный текст
        fullText = text;
        currentPage = 0;

        // Вычисляем количество страниц
        totalPages = Mathf.CeilToInt((float)fullText.Length / CHARS_PER_PAGE);

        // Показываем первую страницу
        ShowCurrentPage();

        dialogueText.textWrappingMode = TextWrappingModes.Normal;
        dialogueText.overflowMode = TextOverflowModes.Overflow;

        continueButton.gameObject.SetActive(true);

        onContinue = continueCallback;
        onPageComplete = null;

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinuePressed);
    }

    /// <summary>
    /// Подсвечивает активного говорящего персонажа.
    /// Неактивный персонаж затемняется.
    /// </summary>
    void UpdateCharacters(string speaker)
    {
        Color active = Color.white;
        Color darker = new Color(0.5f, 0.5f, 0.5f, 1f);

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
        else
        {
            leftCat.color = darker;
            rightSasha.color = darker;
        }
    }

    /// <summary>
    /// Отслеживает нажатия клавиш и клики по панели диалога.
    /// Позволяет продолжать диалог без кнопки.
    /// </summary>
    void Update()
    {
        if (dialoguePanel.activeSelf)
        {
            bool shouldContinue = false;

            // Продолжение по клавишам
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                shouldContinue = true;
            }

            // Продолжение по клику мыши по панели
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

    /// <summary>
    /// Скрывает панель диалога и отключает blocker.
    /// </summary>
    public void Hide()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (blocker != null)
            blocker.SetActive(false);
    }

    /// <summary>
    /// Показывает короткое системное сообщение
    /// без персонажей и с уменьшенной панелью.
    /// </summary>
    public void Message(string speaker, string message, System.Action continueCallback)
    {
        dialoguePanel.SetActive(true);

        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, smallHeight);

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

        // Автоматически закрываем сообщение через 2 секунды
        StartCoroutine(AutoCloseMessage());
    }

    /// <summary>
    /// Автоматически закрывает короткое сообщение
    /// и запускает следующее действие.
    /// </summary>
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

