using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScaler : MonoBehaviour
{
    [Header("Target Size")]
    [SerializeField] private float targetWidth = 20f;
    [SerializeField] private float targetHeight = 11.25f;
    [SerializeField] private bool keepAspectRatio = true;
    [SerializeField] private ScaleMode scaleMode = ScaleMode.FitToScreen;

    [Header("Position")]
    [SerializeField] private bool centerToCamera = true;
    [SerializeField] private Vector3 positionOffset = Vector3.zero;

    public enum ScaleMode
    {
        FitToScreen,
        SetWidth,
        SetHeight,
        SetBoth
    }

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ScaleBackground();

        if (centerToCamera)
        {
            CenterToCamera();
        }
    }

    public void ScaleBackground()
    {
        if (spriteRenderer.sprite == null) return;

        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        Vector3 scale = transform.localScale;

        switch (scaleMode)
        {
            case ScaleMode.FitToScreen:
                Camera cam = Camera.main;
                if (cam != null)
                {
                    float screenHeight = cam.orthographicSize * 2f;
                    float screenWidth = screenHeight * cam.aspect;

                    scale.x = screenWidth / spriteWidth;
                    scale.y = screenHeight / spriteHeight;
                }
                break;

            case ScaleMode.SetWidth:
                scale.x = targetWidth / spriteWidth;
                if (keepAspectRatio)
                {
                    scale.y = scale.x;
                }
                break;

            case ScaleMode.SetHeight:
                scale.y = targetHeight / spriteHeight;
                if (keepAspectRatio)
                {
                    scale.x = scale.y;
                }
                break;

            case ScaleMode.SetBoth:
                scale.x = targetWidth / spriteWidth;
                scale.y = targetHeight / spriteHeight;
                break;
        }

        transform.localScale = scale;
    }

    private void CenterToCamera()
    {
        if (Camera.main == null) return;

        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.z = transform.position.z;

        transform.position = cameraPos + positionOffset;
    }

    // Для обновления позиции при изменении разрешения экрана
    private void OnEnable()
    {
        if (centerToCamera)
        {
            CenterToCamera();
        }
    }
}