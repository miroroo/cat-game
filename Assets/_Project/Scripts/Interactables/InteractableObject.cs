using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Базовый класс для всех интерактивных объектов на сцене.
/// Обрабатывает наведение мыши, клик по объекту,
/// воспроизведение звука и взаимодействие с предметами.
/// </summary>
public class InteractableObject : MonoBehaviour
{ 
    private bool isMouseOver = false;  // Находится ли курсор мыши над объектом
    
    public string objectName;  // Название объекта (для отладки и логов)
    
    public int itemId;   // ID предмета из базы данных Если 0 — объект не является предметом

    private AudioSource musicSource; // Источник воспроизведения звука

    [SerializeField] private AudioClip backgroundMusic;   // Звук, который проигрывается при взаимодействии
    [SerializeField] [Range(0f, 1f)] private float localVolume = 1f;

    [SerializeField] private GameObject highlightObject;   // Подсветка объекта при наведении (при необходимости)
    private SpriteRenderer highlightRenderer;

    /// <summary>
    /// Каждый кадр проверяет положение мыши,
    /// наведение на объект и клик по нему.
    /// </summary>
    void Update()
    {
        // Если главная камера отсутствует — прекращаем выполнение
        if (Camera.main == null)
            return;

        // Получаем позицию мыши в мировых координатах
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Работаем только со слоем interactables
        LayerMask mask = LayerMask.GetMask("interactables");

        // Проверяем, попадает ли курсор на объект
        RaycastHit2D hit = Physics2D.Raycast(
            mousePos,
            Vector2.zero,
            Mathf.Infinity,
            mask
        );

        // Если курсор находится именно над этим объектом
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            // Событие первого наведения
            if (!isMouseOver)
            {
                isMouseOver = true;
                OnMouseEnter();
            }

            // ЛКМ — взаимодействие с объектом
            if (Input.GetMouseButtonDown(0))
            {
                Interact();
            }
        }
        else
        {
            // Если курсор ушёл с объекта
            if (isMouseOver)
            {
                isMouseOver = false;
                OnMouseExit();
            }
        }
    }

    /// <summary>
    /// Инициализирует AudioSource для воспроизведения звука.
    /// Если компонента нет — создаёт его автоматически.
    /// </summary>
    private void Start()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
        // Пытаемся получить существующий AudioSource
        musicSource = GetComponent<AudioSource>();

        // Если его нет — создаём новый
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        // Настройка источника звука
        musicSource.playOnAwake = false;
        musicSource.clip = backgroundMusic;
    }

    /// <summary>
    /// Вызывается при наведении курсора на объект.
    /// Можно использовать для подсветки.
    /// </summary>
    private void OnMouseEnter()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(true);
        }
    }

    /// <summary>
    /// Вызывается при уходе курсора с объекта.
    /// Можно отключать подсветку.
    /// </summary>
    private void OnMouseExit()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
    }

    /// <summary>
    /// Основной метод взаимодействия с объектом.
    /// Воспроизводит звук и обрабатывает предметы.
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log("Взаимодействие с " + objectName);

        // Если назначен звук — воспроизводим его
        if (backgroundMusic != null)
        {
            musicSource.PlayOneShot(backgroundMusic);
        }

        // Если объект является предметом
        if (itemId > 0)
        {
            // Получаем предмет из базы по ID
            Item item = ItemDatabase.GetItem(itemId);

            if (item != null)
            {
                // Проверяем, можно ли взять предмет
                if (item.isTakeable)
                {
                    // Скрываем объект со сцены
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Этот предмет нельзя взять");
                }
            }
            else
            {
                Debug.LogError(
                    $"Предмет с ID {itemId} не найден в базе"
                );
            }
        }
    }
}