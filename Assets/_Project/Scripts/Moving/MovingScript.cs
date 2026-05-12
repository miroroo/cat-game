using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer sprite;
    private bool hasNotified = false;

    // Кэшируем клавиши для производительности
    private KeyCode currentLeftKey;
    private KeyCode currentRightKey;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        // Загружаем сохранённые клавиши
        LoadSavedKeys();
    }

    private void LoadSavedKeys()
    {
        // Читаем напрямую из PlayerPrefs
        currentLeftKey = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)KeyCode.LeftArrow);
        currentRightKey = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)KeyCode.RightArrow);

        Debug.Log($"Загружены клавиши: Лево = {currentLeftKey}, Право = {currentRightKey}");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F12))
        {
            LoadSavedKeys();
            Debug.Log("Принудительно перезагрузили клавиши");
        }

        movement.x = 0f;
        movement.y = 0f;

        // Используем загруженные клавиши
        if (Input.GetKey(currentLeftKey) || Input.GetKey(KeyCode.A))
            movement.x = -1f;
        else if (Input.GetKey(currentRightKey) || Input.GetKey(KeyCode.D))
            movement.x = 1f;

        bool isMoving = movement.x != 0;
        if (animator != null)
            animator.SetBool("isMoving", isMoving);

        if (sprite != null && movement.x != 0)
            sprite.flipX = movement.x < 0;

        if (!hasNotified && movement.x != 0)
        {
            hasNotified = true;
            var tutorial = FindObjectOfType<TutorialController>();
            if (tutorial != null)
                tutorial.OnPlayerMoved();
        }
    }

    void FixedUpdate()
    {
        if (movement.magnitude < 0.2f)
            movement = Vector2.zero;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Опционально: метод для обновления клавиш без перезапуска
    private void OnEnable()
    {
        // Обновляем клавиши каждый раз когда объект активируется
        LoadSavedKeys();
    }
}