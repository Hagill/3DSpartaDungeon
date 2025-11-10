using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable,
}

public enum ConsumableType
{
    Health,
    Stamina,
    // 그 외 2단 점프나 무적 등
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefabs;

    [Header("Stacking")]
    public bool canStack;
    public int maxStack;

    [Header("Stacking")]
    public ItemDataConsumable[] consumables;
}
