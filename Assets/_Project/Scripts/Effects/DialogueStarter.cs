using UnityEngine;

public class SceneDialogueStarter : MonoBehaviour
{
    private void Start()
    {
        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.Show(
                "",
                "Длинный коридор здания. Пол покрыт плиткой, местами на полу виднеются следы каких-то жидкостей. " +
                "На стенах стенды с объявлениями:\n" +
                "День открытых дверей, потерялась кошка...",
                StartMainDialogue
            );
        }
    }

    private void StartMainDialogue()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(15);
        }
    }
}