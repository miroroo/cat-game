using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer sprite;

    private KeyCode leftKey;
    private KeyCode rightKey;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        // Загружаем сохранённые клавиши из PlayerPrefs
        leftKey = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)KeyCode.LeftArrow);
        rightKey = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)KeyCode.RightArrow);
    }

    void Update()
    {
        movement = Vector2.zero;

        if (Input.GetKey(leftKey))
            movement.x = -1f;
        else if (Input.GetKey(rightKey))
            movement.x = 1f;

        bool isMoving = movement.x != 0;
        animator.SetBool("isMoving", isMoving);

        if (movement.x > 0)
            sprite.flipX = false;
        else if (movement.x < 0)
            sprite.flipX = true;
    }

    void FixedUpdate()
    {
        if (movement.magnitude < 0.2f)
            movement = Vector2.zero;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}