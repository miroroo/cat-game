using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeysSettings : MonoBehaviour
{
    [Header("Кнопки для переназначения")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button skipButton;

    [Header("Текст на кнопках")]
    [SerializeField] private Text leftButtonText;
    [SerializeField] private Text rightButtonText;
    [SerializeField] private Text skipButtonText;

    [Header("Клавиши по умолчанию")]
    [SerializeField] private KeyCode defaultLeftKey = KeyCode.LeftArrow;
    [SerializeField] private KeyCode defaultRightKey = KeyCode.RightArrow;
    [SerializeField] private KeyCode defaultSkipKey = KeyCode.Space;

    private bool isRebinding = false;
    private Button currentButton = null;
    private string currentAction = "";

    private KeyCode currentLeftKey;
    private KeyCode currentRightKey;
    private KeyCode currentSkipKey;

    private void Start()
    {
        LoadKeyBindings();
        UpdateButtonTexts();
        SetupButtonListeners();
    }

    private void LoadKeyBindings()
    {
        // PlayerPrefs.GetInt - достаём число из памяти
        // (int)defaultLeftKey - преобразуем клавишу в число для сохранения
        // (KeyCode) - преобразуем число обратно в клавишу

        currentLeftKey = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)defaultLeftKey);
        currentRightKey = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)defaultRightKey);
        currentSkipKey = (KeyCode)PlayerPrefs.GetInt("SkipKey", (int)defaultSkipKey);
    }

    private void SaveKeyBindings()
    {
        // Сохраняем клавиши как числа в память
        PlayerPrefs.SetInt("LeftKey", (int)currentLeftKey);
        PlayerPrefs.SetInt("RightKey", (int)currentRightKey);
        PlayerPrefs.SetInt("SkipKey", (int)currentSkipKey);
        PlayerPrefs.Save();

        Debug.Log("Клавиши сохранены: Влево=" + currentLeftKey +
                  ", Вправо=" + currentRightKey +
                  ", Пропуск=" + currentSkipKey);
    }

    private void SetupButtonListeners()
    {
        // обработчики нажатий на каждую кнопку
        if (leftButton != null)
            leftButton.onClick.AddListener(() => StartRebinding("Left", leftButton));

        if (rightButton != null)
            rightButton.onClick.AddListener(() => StartRebinding("Right", rightButton));

        if (skipButton != null)
            skipButton.onClick.AddListener(() => StartRebinding("Skip", skipButton));
    }

    private void UpdateButtonTexts()
    {
        if (leftButtonText != null)
            leftButtonText.text = GetKeyDisplayName(currentLeftKey);

        if (rightButtonText != null)
            rightButtonText.text = GetKeyDisplayName(currentRightKey);

        if (skipButtonText != null)
            skipButtonText.text = GetKeyDisplayName(currentSkipKey);
    }

    // убрать? 
    private string GetKeyDisplayName(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Space: return "Пробел";
            case KeyCode.Return: return "Enter";
            case KeyCode.LeftShift: return "Shift (Левый)";
            case KeyCode.RightShift: return "Shift (Правый)";
            case KeyCode.LeftControl: return "Ctrl (Левый)";
            case KeyCode.RightControl: return "Ctrl (Правый)";
            case KeyCode.LeftAlt: return "Alt (Левый)";
            case KeyCode.RightAlt: return "Alt (Правый)";
            case KeyCode.Tab: return "Tab";
            case KeyCode.Escape: return "Esc";
            case KeyCode.Backspace: return "Backspace";
            default: return key.ToString();
        }
    }

    // Начинаем переназначение клавиши
    private void StartRebinding(string actionName, Button button)
    {
        if (isRebinding)
        {
            Debug.Log("Сначала завершите текущее переназначение");
            return;
        }

        isRebinding = true;
        currentAction = actionName;
        currentButton = button;

        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = "..."; // Троеточие означает ожидание. убрать?
        }

        Debug.Log($"Ожидание нажатия клавиши для действия: {actionName}");
    }

    // Update вызывается каждый кадр
    private void Update()
    {
        if (!isRebinding) return;

        // Проверяем нажатие любой клавиши
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (key == KeyCode.Mouse0 || key == KeyCode.Mouse1 || key == KeyCode.Mouse2)
                    {
                        Debug.Log("Нельзя назначить кнопку мыши");
                        continue;
                    }

                    if (key == KeyCode.LeftShift || key == KeyCode.RightShift ||
                        key == KeyCode.LeftControl || key == KeyCode.RightControl ||
                        key == KeyCode.LeftAlt || key == KeyCode.RightAlt)
                    {
                        Debug.Log("Лучше не использовать клавиши-модификаторы по отдельности");
                        continue;
                    }

                    AssignNewKey(key);
                    break;
                }
            }
        }
    }

    // Назначаем новую клавишу для действия
    private void AssignNewKey(KeyCode newKey)
    {
        if (!IsKeyAvailable(newKey)) // если кнопка уже используется
        {
            Text buttonText = currentButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Уже используется!";
            }

            Debug.LogWarning($"Клавиша {newKey} уже используется другим действием");

            // Через 1 секунду возвращаем нормальный текст (без переназначения)
            StartCoroutine(RestoreButtonTextAfterError());
            return;
        }

        // Назначаем новую клавишу в зависимости от выбранного действия
        switch (currentAction)
        {
            case "Left":
                currentLeftKey = newKey;
                Debug.Log($"Клавиша 'Влево' изменена на {GetKeyDisplayName(newKey)}");
                break;

            case "Right":
                currentRightKey = newKey;
                Debug.Log($"Клавиша 'Вправо' изменена на {GetKeyDisplayName(newKey)}");
                break;

            case "Skip":
                currentSkipKey = newKey;
                Debug.Log($"Клавиша 'Пропустить' изменена на {GetKeyDisplayName(newKey)}");
                break;
        }

        SaveKeyBindings();

        UpdateButtonTexts();

        isRebinding = false;
        currentButton = null;
        currentAction = "";

        // Останавливаем таймаут
        StopAllCoroutines();
    }

    // Восстанавливаем текст на кнопке после ошибки
    private IEnumerator RestoreButtonTextAfterError()
    {
        yield return new WaitForSeconds(1f); // Ждём 1 секунду

        if (currentButton != null)
        {
            UpdateButtonTexts();
        }

        isRebinding = false;
        currentButton = null;
        currentAction = "";
    }

    // Проверяет, свободна ли клавиша (не используется другими действиями)
    private bool IsKeyAvailable(KeyCode key)
    {
        // (игрок нажал ту же самую клавишу, ничего не меняем)
        if (currentAction == "Left" && currentLeftKey == key) return true;
        if (currentAction == "Right" && currentRightKey == key) return true;
        if (currentAction == "Skip" && currentSkipKey == key) return true;

        // Проверяем, не используется ли клавиша в других действиях
        if (currentAction != "Left" && currentLeftKey == key) return false;
        if (currentAction != "Right" && currentRightKey == key) return false;
        if (currentAction != "Skip" && currentSkipKey == key) return false;

        return true;
    }

    // публичные методы для доступа из других скриптов. MovingScript?
    public KeyCode GetLeftKey() => currentLeftKey;
    public KeyCode GetRightKey() => currentRightKey;
    public KeyCode GetSkipKey() => currentSkipKey;
}