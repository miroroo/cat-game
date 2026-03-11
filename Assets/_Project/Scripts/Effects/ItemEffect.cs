using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public string effectName; // для удобства
    public Sprite icon; // опционально

    // Метод, который будет вызываться при использовании предмета
    // user — тот, кто использует предмет (например, игрок)
    public abstract void Apply(GameObject user);
}