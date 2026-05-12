using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance { get; private set; }

    private Image brightnessOverlay;
    private GameObject overlayObject;

    [Range(0f, 1f)]
    public float currentBrightness = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateOverlay();
    }

    private void CreateOverlay()
    {
        // Ждём появления Canvas
        Invoke(nameof(FindAndCreateOverlay), 0.1f);
    }

    private void FindAndCreateOverlay()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas не найден, яркость не будет работать!");
            return;
        }

        // Создаём оверлей
        overlayObject = new GameObject("BrightnessOverlay");
        overlayObject.transform.SetParent(canvas.transform);

        RectTransform rect = overlayObject.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;

        brightnessOverlay = overlayObject.AddComponent<Image>();
        brightnessOverlay.color = new Color(0, 0, 0, 0);
        brightnessOverlay.raycastTarget = false;

        overlayObject.transform.SetAsFirstSibling();

        // Загружаем сохранённую яркость
        currentBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        SetBrightness(currentBrightness);

        Debug.Log("BrightnessManager создан и живёт между сценами");
    }

    public void SetBrightness(float value)
    {
        currentBrightness = Mathf.Clamp01(value);

        if (brightnessOverlay != null)
        {
            float darkness = Mathf.Clamp01(1 - currentBrightness) * 0.7f;
            Color color = brightnessOverlay.color;
            color.a = darkness;
            brightnessOverlay.color = color;
        }

        PlayerPrefs.SetFloat("Brightness", currentBrightness);
        PlayerPrefs.Save();
    }

    public float GetBrightness() => currentBrightness;
}