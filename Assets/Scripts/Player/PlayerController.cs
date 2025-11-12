using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpForce;
    public float useJumpStamina;
    private Vector2 currentMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform fpCamObj;
    public Transform tpCamObj;
    public float fpMinXLook;
    public float fpMaxXLook;
    public float tpMinXLook;
    public float tpMaxXLook;
    private float fpCamCurXRot;
    private float tpCamCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    [Header("Camera")]
    public Camera fpCamera;
    public Camera tpCamera;
    public Image crossHair;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private bool isFirstPerson = true;

    // 이동 중인 상태
    public bool isMoving;
    public bool isRunning;

    
    public Action inventory;

    private PlayerCondition condition;
    private bool didDoubleJump = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        condition = GetComponent<PlayerCondition>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        isMoving = currentMovementInput.magnitude > 0.1f;

        _animator.SetBool("Walk", isMoving && !isRunning);
        _animator.SetBool("Run", isMoving && isRunning);
        _animator.SetBool("IsGrounded", IsGrounded());
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            playerLook();
        }
    }

    void Move()
    {
        // 방향값
        Vector3 dir = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;
        // 이동속도
        float speed = isRunning ? runSpeed : moveSpeed;
        speed *= condition.speedBoostMultiplier;
        dir *= speed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void playerLook()
    {
        if (isFirstPerson)
        {
            fpCamCurXRot += mouseDelta.y * lookSensitivity;
            fpCamCurXRot = Mathf.Clamp(fpCamCurXRot, fpMinXLook, fpMaxXLook);
            fpCamObj.localEulerAngles = new Vector3(-fpCamCurXRot, 0, 0);

            transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
        }
        else
        {
            tpCamCurXRot += mouseDelta.y * lookSensitivity;
            tpCamCurXRot = Mathf.Clamp(tpCamCurXRot, tpMinXLook, tpMaxXLook);
            tpCamObj.localEulerAngles = new Vector3(-tpCamCurXRot, 0, 0);

            transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // InputActionPhase.Started 키가 눌렸을 때 한 번, Performed 누를 때 동안 계속 <- 이걸로 토글, 유지 설정 변경 가능할 듯
        if (context.phase == InputActionPhase.Performed)
        {
            currentMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            currentMovementInput = Vector2.zero;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isRunning = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            if(IsGrounded() && condition.UseStamina(useJumpStamina))
            {
                _animator.SetTrigger("Jump");
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            }
            else if(!IsGrounded() && !didDoubleJump && condition.isDoubleJump && condition.UseStamina(useJumpStamina))
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
                _animator.SetTrigger("Jump");

                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
                didDoubleJump = true;
            }
        }
    }

    bool IsGrounded()
    {

        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                didDoubleJump = false;
                return true;
            }
        }

        return false;
    }

    public void OnCameraToggle(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isFirstPerson = !isFirstPerson; // 모드 전환

            fpCamera.gameObject.SetActive(isFirstPerson);
            tpCamera.gameObject.SetActive(!isFirstPerson);
            crossHair.gameObject.SetActive(isFirstPerson);
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
