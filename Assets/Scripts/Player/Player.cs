using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovements movements;
    public PlayerCondition condition;

    public ItemData itemData;
    public Action addItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        movements = GetComponent<PlayerMovements>();
        condition = GetComponent<PlayerCondition>();
    }
}
