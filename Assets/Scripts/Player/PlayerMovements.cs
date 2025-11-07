using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 currentMovementInput;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // 방향값
        Vector3 dir = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;
        // 이동속도
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // InputActionPhase.Started 키가 눌리기 시작했을 때
        if (context.phase == InputActionPhase.Performed)
        {
            currentMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            currentMovementInput = Vector2.zero;
        }
    }
}
