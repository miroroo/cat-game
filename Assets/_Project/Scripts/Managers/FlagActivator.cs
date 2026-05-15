using UnityEngine;

public class FlagActivator : MonoBehaviour
{
    [SerializeField] private string flag;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private bool destroyAfterActivation = true; // Удалить после активации

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (FlagManager.Instance != null && FlagManager.Instance.GetFlag(flag))
        {
            if (targetObject != null)
                targetObject.SetActive(true);

            if (destroyAfterActivation)
                Destroy(gameObject);
        }
    }
}