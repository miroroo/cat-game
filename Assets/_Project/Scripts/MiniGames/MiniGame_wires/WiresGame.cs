using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresGame : MonoBehaviour
{
    [System.Serializable]
    public class Wire
    {
        public SpriteRenderer sprite;
        public Color color;
        [HideInInspector] public bool connected;
    }

    public List<Wire> wires = new List<Wire>();
    public float showTime = 1f;
    public float delay = 0.3f;
    public float connectTime = 3f;
    public string sceneToLoad = "NextScene"; // Имя сцены для загрузки при победе
    public string gameOverScene = "GameOver"; // Имя сцены для загрузки при поражении
    public int maxFailures = 5; // Максимальное количество неудачных попыток

    private List<Wire> order = new List<Wire>();
    private int index = 0;
    private bool playing = false;
    private Coroutine timer;
    private bool isWaitingForRestart = false;
    private int failureCount = 0; // Счетчик неудачных попыток


    void Start()
    {
        foreach (Wire w in wires)
        {
            w.sprite.color = Color.gray;
            if (w.sprite.GetComponent<Collider2D>() == null)
                w.sprite.gameObject.AddComponent<BoxCollider2D>();

            WireClickHandler h = w.sprite.GetComponent<WireClickHandler>();
            if (h == null) h = w.sprite.gameObject.AddComponent<WireClickHandler>();
            h.Init(this, w);
        }
        StartGame();
    }

    void StartGame()
    {
        StopAllCoroutines();
        playing = true;
        isWaitingForRestart = false;
        index = 0;
        foreach (Wire w in wires)
        {
            w.connected = false;
            w.sprite.color = Color.gray;
        }
        StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        order = new List<Wire>(wires);
        for (int i = 0; i < order.Count; i++)
        {
            int r = Random.Range(i, order.Count);
            Wire t = order[i];
            order[i] = order[r];
            order[r] = t;
        }

        foreach (Wire w in order)
        {
            w.sprite.color = w.color;
            yield return new WaitForSeconds(showTime);
            w.sprite.color = Color.gray;
            yield return new WaitForSeconds(delay);
        }

        NextWire();
    }

    void NextWire()
    {
        if (!playing) return;
        if (index >= order.Count) { Win(); return; }

        if (timer != null) StopCoroutine(timer);
        timer = StartCoroutine(Timeout());

        order[index].sprite.color = Color.white;
    }

    IEnumerator Timeout()
    {
        float t = 0;
        while (t < connectTime)
        {
            // ===== ДОБАВИТЬ ЭТУ ПРОВЕРКУ =====
            if (index >= order.Count || index < 0)
            {
                yield break;
            }
            // ================================

            if (order[index].connected) yield break;
            t += Time.deltaTime;
            yield return null;
        }

        // ===== ДОБАВИТЬ ЭТУ ПРОВЕРКУ =====
        if (index < order.Count)
        {
            Lose();
        }
        // ================================
    }

    public void TryConnect(Wire clicked)
    {
        if (!playing || clicked.connected) return;

        // ===== ДОБАВИТЬ ЭТУ ПРОВЕРКУ =====
        if (index >= order.Count)
        {
            Win();
            return;
        }
        // ================================

        if (clicked == order[index])
        {
            clicked.connected = true;
            clicked.sprite.color = Color.green;
            index++;
            NextWire();
        }
        else
        {
            StartCoroutine(FlashRed(clicked));
            Lose();
        }
    }

    IEnumerator FlashRed(Wire w)
    {
        w.sprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        if (!w.connected) w.sprite.color = Color.gray;
    }

    void Win()
    {
        playing = false;

        // Загружаем следующую локацию
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadLocation(sceneToLoad);
        }
        else
        {
            Debug.LogError("SceneLoader.Instance не найден!");
        }
    }

    void Lose()
    {
        if (isWaitingForRestart) return; // Предотвращаем множественные перезапуски

        playing = false;
        isWaitingForRestart = true;

        // Увеличиваем счетчик неудач
        failureCount++;

        // Проверяем, достигнут ли лимит неудач
        if (failureCount >= maxFailures)
        {
            // Переходим на сцену GameOver
            if (SceneLoader.Instance != null)
            {
                DatabaseManager.Instance.ReloadDatabase();
                SceneLoader.Instance.LoadLocation(gameOverScene);
            }
            else
            {
                Debug.LogError("SceneLoader.Instance не найден!");
                // Альтернативный способ загрузки сцены
                UnityEngine.SceneManagement.SceneManager.LoadScene(gameOverScene);
            }
            return;
        }

        // Запускаем перезапуск игры через небольшую задержку
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        // Небольшая задержка перед перезапуском, чтобы игрок видел ошибку
        yield return new WaitForSeconds(1f);

        // Останавливаем все активные корутины
        StopAllCoroutines();

        // Перезапускаем игру
        StartGame();
    }

    // Опционально: метод для сброса счетчика неудач (например, при победе)
    private void ResetFailureCount()
    {
        failureCount = 0;
    }
}