using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovements movements;
    public PlayerCondition condition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        movements = GetComponent<PlayerMovements>();
        condition = GetComponent<PlayerCondition>();
    }
}
