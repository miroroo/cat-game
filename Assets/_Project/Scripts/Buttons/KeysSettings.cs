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

		Text buttonText = button.GetComponentInChildren<Text>();
		if (buttonText != null)
		{
			buttonText.text = "...";
		}

		Debug.Log($"Ожидание нажатия клавиши для действия: {actionName}");
	}

	private void Update()
	{
		if (!isRebinding) return;

		if (Input.anyKeyDown)
		{
			foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown(key))
				{
					// Можно упростить проверку мыши
					if (key == KeyCode.Mouse0 || key == KeyCode.Mouse1 || key == KeyCode.Mouse2)
					{
						continue; // Просто игнорируем, без сообщения
					}

					// Упрощённая проверка модификаторов
					if (key == KeyCode.LeftShift || key == KeyCode.RightShift ||
						key == KeyCode.LeftControl || key == KeyCode.RightControl ||
						key == KeyCode.LeftAlt || key == KeyCode.RightAlt)
					{
						continue; // Игнорируем без сообщения
					}

					AssignNewKey(key);
					break;
				}
			}
		}
	}

	private void AssignNewKey(KeyCode newKey)
	{
		if (!IsKeyAvailable(newKey))
		{
			Text buttonText = currentButton.GetComponentInChildren<Text>();
			if (buttonText != null)
			{
				buttonText.text = "Уже используется!";
			}
			StartCoroutine(RestoreButtonTextAfterError());
			return;
		}

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

		isRebinding = false;
		currentButton = null;
		currentAction = "";
		StopAllCoroutines();
	}

	private IEnumerator RestoreButtonTextAfterError()
	{
		yield return new WaitForSeconds(1f);

		if (currentButton != null)
		{
			UpdateButtonTexts(); // обновляем все тексты
		}

		isRebinding = false;
		currentButton = null;
		currentAction = "";
	}

	private bool IsKeyAvailable(KeyCode key)
	{
		if (currentAction == "Left" && currentLeftKey == key) return true;
		if (currentAction == "Right" && currentRightKey == key) return true;
		if (currentAction == "Skip" && currentSkipKey == key) return true;

		if (currentAction != "Left" && currentLeftKey == key) return false;
		if (currentAction != "Right" && currentRightKey == key) return false;
		if (currentAction != "Skip" && currentSkipKey == key) return false;

		return true;
	}


	public KeyCode GetLeftKey()
	{
		return currentLeftKey;
	}

	public KeyCode GetRightKey()
	{
		return currentRightKey;
	}

	public KeyCode GetSkipKey()
	{
		return currentSkipKey;
	}
}