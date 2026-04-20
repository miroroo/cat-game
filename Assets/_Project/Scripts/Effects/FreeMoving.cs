using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoWalkCharacter : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform doorTransform; // Ссылка на дверь
    [SerializeField] private Vector2 doorPosition;   // Координаты двери

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string walkParameter = "isMoving";

    [Header("Next Scene")]
    [SerializeField] private string sceneToLoad = "NextLocation";

    [Header("Required Flag")]
    [SerializeField] private string requiredFlag = "talked_to_cat_loc2";

    private bool isMoving = true;
    private bool isAtDoor = false;
    private bool waitingForDialogueEnd = false;
    private float startY; // Сохраняем начальную Y позицию

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        // Сохраняем начальную Y позицию (чтобы герой не двигался вверх/вниз)
        startY = transform.position.y;

        // Если не задана дверь, ищем по тегу
        if (doorTransform == null)
        {
            GameObject door = GameObject.FindGameObjectWithTag("Door");
            if (door != null)
                doorTransform = door.transform;
        }

        // Если дверь найдена, берём её позицию (только X координату)
        if (doorTransform != null)
        {
            doorPosition = doorTransform.position;
            doorPosition.y = startY; // Фиксируем Y на уровне героя
        }
        else
        {
            // Если дверь не найдена, используем только X из заданных координат
            doorPosition.y = startY;
        }
    }

    private void Update()
    {
        if (!isMoving) return;

        // Двигаемся ТОЛЬКО по оси X
        Vector2 currentPos = transform.position;
        Vector2 targetPos = new Vector2(doorPosition.x, startY); // Y всегда фиксирован

        // Движение только по X
        float newX = Mathf.MoveTowards(currentPos.x, targetPos.x, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, startY, transform.position.z);

        // Анимация
        if (animator != null)
            animator.SetBool(walkParameter, true);

        // Проверяем, дошли ли до цели (только по X)
        float distanceX = Mathf.Abs(transform.position.x - doorPosition.x);

        if (distanceX < 0.05f)
        {
            isMoving = false;

            if (animator != null)
                animator.SetBool(walkParameter, false);

            // Дошел до двери - пробуем открыть
            TryOpenDoor();
        }
    }

    private void TryOpenDoor()
    {
        if (isAtDoor) return;
        isAtDoor = true;

        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }

        bool hasRequiredFlag = FlagManager.Instance.GetFlag(requiredFlag);

        if (!hasRequiredFlag)
        {
            Debug.Log("Нужно сначала поговорить с котом!");
            return;
        }

        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог идёт, жду окончания...");
            waitingForDialogueEnd = true;
            StartCoroutine(WaitForDialogueEnd());
        }
        else
        {
            LoadNextScene();
        }
    }

    private IEnumerator WaitForDialogueEnd()
    {
        while (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        waitingForDialogueEnd = false;
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        Debug.Log($"Загружаем сцену: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }

    // Визуализация в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        // Показываем целевую позицию на уровне Y героя
        if (Application.isPlaying)
        {
            Vector3 targetPos = new Vector3(doorPosition.x, startY, 0);
            Gizmos.DrawWireSphere(targetPos, 0.2f);
            Gizmos.DrawLine(transform.position, targetPos);
        }
        else
        {
            Gizmos.DrawWireSphere(doorPosition, 0.2f);
        }
    }
}