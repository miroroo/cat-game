using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Чтобы поля отображались в инспекторе
public class Item
{
    public string itemName;
    public string description;
    public Sprite sprite; 

    // Конструктор для удобства
    public Item(string name, string desc, Sprite spr = null)
    {
        itemName = name;
        description = desc;
        sprite = spr;
    }
}