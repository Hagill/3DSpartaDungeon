using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovements movements;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        movements = GetComponent<PlayerMovements>();
    }
}
