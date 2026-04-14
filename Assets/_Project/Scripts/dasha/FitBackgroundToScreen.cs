using UnityEngine;

public class FitBackgroundToScreen : MonoBehaviour
{
    void Start()
    {
        // Получаем спрайт
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Получаем размеры камеры
        Camera cam = Camera.main;
        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;

        // Получаем размеры спрайта
        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        // Вычисляем масштаб
        float scaleX = screenWidth / spriteWidth;
        float scaleY = screenHeight / spriteHeight;

        // Выбираем максимальный масштаб
        float scale = Mathf.Max(scaleX, scaleY);

        // Применяем масштаб
        transform.localScale = new Vector3(scale, scale, 1);

        // Центрируем фон
        transform.position = new Vector3(0, 0, 0);
    }
}