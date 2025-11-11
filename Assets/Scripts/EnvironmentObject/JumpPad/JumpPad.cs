using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Jump")]
    public float jumpForce;

    public Player player;
    public PlayerController controller;

    public Animator _animator;
    public Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        player = CharacterManager.Instance.Player;
        controller = CharacterManager.Instance.Player.controller;

        _rigidbody = controller.GetComponent<Rigidbody>();
        _animator = controller.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player.gameObject)
        {
            _animator.SetTrigger("Jump");
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }
}
