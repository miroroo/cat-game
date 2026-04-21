using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [Header("Настройки двери")]
    [SerializeField] private string sceneToLoad = "Coridor";
    [SerializeField] private string requiredFlag = ""; // Пусто = не нужен флаг
    [SerializeField] private string lockedMessage = "Дверь закрыта";
    [SerializeField] private string unlockedMessage = "Дверь открыта";
    
    private bool waitingForDialogueEnd = false;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;
        
        if (FlagManager.Instance == null)
        {
            Debug.LogError("FlagManager не найден!");
            return;
        }
        
        if (waitingForDialogueEnd)
            return;
        
        // Проверка на активный диалог
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("Диалог идёт, жду окончания...");
            waitingForDialogueEnd = true;
            return;
        }
        
        // Проверка на нужный флаг
        bool hasRequiredFlag = string.IsNullOrEmpty(requiredFlag) || FlagManager.Instance.GetFlag(requiredFlag);
        
        if (hasRequiredFlag)
        {
            // Дверь открыта
            if (!string.IsNullOrEmpty(unlockedMessage))
                DialogueUI.Instance.Message("", unlockedMessage, null);
            
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            // Дверь закрыта
            DialogueUI.Instance.Message("", lockedMessage, null);
        }
    }
    
    private void Update()
    {
        if (waitingForDialogueEnd && DialogueManager.Instance != null && !DialogueManager.Instance.IsDialogueActive)
        {
            waitingForDialogueEnd = false;
            Debug.Log("Диалог завершён, выполняю переход!");
            
            // Проверяем флаг после диалога
            bool hasRequiredFlag = string.IsNullOrEmpty(requiredFlag) || FlagManager.Instance.GetFlag(requiredFlag);
            
            if (hasRequiredFlag)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
