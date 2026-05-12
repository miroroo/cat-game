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
        currentLeftKey = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)defaultLeftKey);
        currentRightKey = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)defaultRightKey);
        currentSkipKey = (KeyCode)PlayerPrefs.GetInt("SkipKey", (int)defaultSkipKey);
    }

    private void SaveKeyBindings()
    {
        PlayerPrefs.SetInt("LeftKey", (int)currentLeftKey);
        PlayerPrefs.SetInt("RightKey", (int)currentRightKey);
        PlayerPrefs.SetInt("SkipKey", (int)currentSkipKey);
        PlayerPrefs.Save();
    }

    private void SetupButtonListeners()
    {
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

    private string GetKeyDisplayName(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Space: return "Пробел";
            case KeyCode.Return: return "Enter";
            case KeyCode.LeftShift: return "Shift";
            case KeyCode.RightShift: return "Shift";
            case KeyCode.LeftControl: return "Ctrl";
            case KeyCode.RightControl: return "Ctrl";
            case KeyCode.LeftAlt: return "Alt";
            case KeyCode.RightAlt: return "Alt";
            case KeyCode.Tab: return "Tab";
            case KeyCode.Escape: return "Esc";
            case KeyCode.Backspace: return "Backspace";
            case KeyCode.UpArrow: return "↑";
            case KeyCode.DownArrow: return "↓";
            case KeyCode.LeftArrow: return "←";
            case KeyCode.RightArrow: return "→";
            default: return key.ToString();
        }
    }

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

        // Меняем текст на кнопке
        Text buttonText = null;
        switch (actionName)
        {
            case "Left":
                buttonText = leftButtonText;
                break;
            case "Right":
                buttonText = rightButtonText;
                break;
            case "Skip":
                buttonText = skipButtonText;
                break;
        }

        if (buttonText != null)
        {
            buttonText.text = "Нажми любую клавишу...";
        }

        Debug.Log($"Ожидание нажатия клавиши для действия: {actionName}");

        // Запускаем корутину для ожидания нажатия
        StartCoroutine(WaitForKeyPress());
    }

    private IEnumerator WaitForKeyPress()
    {
        // Ждём, пока игрок нажмёт клавишу
        while (isRebinding)
        {
            // Проверяем все возможные клавиши
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    // Пропускаем клавиши мыши
                    if (key == KeyCode.Mouse0 || key == KeyCode.Mouse1 || key == KeyCode.Mouse2 ||
                        key == KeyCode.Mouse3 || key == KeyCode.Mouse4 || key == KeyCode.Mouse5)
                    {
                        Debug.Log("Клавиши мыши нельзя использовать");
                        break;
                    }

                    // Пропускаем клавиши-модификаторы (если используете отдельно)
                    if (key == KeyCode.LeftShift || key == KeyCode.RightShift ||
                        key == KeyCode.LeftControl || key == KeyCode.RightControl ||
                        key == KeyCode.LeftAlt || key == KeyCode.RightAlt)
                    {
                        Debug.Log("Клавиши-модификаторы нельзя использовать отдельно");
                        break;
                    }

                    // Назначаем клавишу
                    AssignNewKey(key);
                    yield break;
                }
            }
            yield return null; // Ждём следующий кадр
        }
    }

    private void AssignNewKey(KeyCode newKey)
    {
        Debug.Log($"Попытка назначить клавишу: {newKey} для действия {currentAction}");

        if (!IsKeyAvailable(newKey))
        {
            Debug.Log($"Клавиша {newKey} уже используется!");

            // Восстанавливаем текст на кнопке
            Text buttonText = GetButtonTextForAction(currentAction);
            if (buttonText != null)
            {
                buttonText.text = "Уже используется!";
            }

            StartCoroutine(RestoreButtonTextAfterError());
            return;
        }

        // Сохраняем новую клавишу
        switch (currentAction)
        {
            case "Left":
                currentLeftKey = newKey;
                break;
            case "Right":
                currentRightKey = newKey;
                break;
            case "Skip":
                currentSkipKey = newKey;
                break;
        }

        SaveKeyBindings();
        UpdateButtonTexts();

        Debug.Log($"Клавиша {newKey} назначена для действия {currentAction}");

        isRebinding = false;
        currentButton = null;
        currentAction = "";
    }

    private IEnumerator RestoreButtonTextAfterError()
    {
        yield return new WaitForSeconds(1f);

        if (isRebinding)
        {
            UpdateButtonTexts();
            isRebinding = false;
            currentButton = null;
            currentAction = "";
        }
    }

    private Text GetButtonTextForAction(string action)
    {
        switch (action)
        {
            case "Left": return leftButtonText;
            case "Right": return rightButtonText;
            case "Skip": return skipButtonText;
            default: return null;
        }
    }

    private bool IsKeyAvailable(KeyCode key)
    {
        // Разрешаем использовать ту же клавишу, если она принадлежит текущему действию
        if (currentAction == "Left" && currentLeftKey == key) return true;
        if (currentAction == "Right" && currentRightKey == key) return true;
        if (currentAction == "Skip" && currentSkipKey == key) return true;

        // Проверяем, не используется ли клавиша другим действием
        if (currentAction != "Left" && currentLeftKey == key) return false;
        if (currentAction != "Right" && currentRightKey == key) return false;
        if (currentAction != "Skip" && currentSkipKey == key) return false;

        return true;
    }

    public KeyCode GetLeftKey() => currentLeftKey;
    public KeyCode GetRightKey() => currentRightKey;
    public KeyCode GetSkipKey() => currentSkipKey;
}