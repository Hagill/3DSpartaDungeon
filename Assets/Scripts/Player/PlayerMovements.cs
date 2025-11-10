using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovements : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpForce;
    private Vector2 currentMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform camObj;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    [Header("Camera")]
    public Camera fpCamera;
    public Camera tpCamera;
    public Image crossHair;

    private Rigidbody _rigidbody;
    private Animator _animator;
    // 이동 중인 상태
    private bool isMoving;
    private bool isRunning;

    public bool isFirstPerson = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
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
        playerLook();
    }

    void Move()
    {
        // 방향값
        Vector3 dir = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;
        // 이동속도
        float speed = isRunning ? runSpeed : moveSpeed;
        dir *= speed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void playerLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        camObj.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // InputActionPhase.Started 키가 눌렸을 때 한 번, Performed 누를 때 동안 계속 <- 이걸로 토글, 유지 설정 변경 가능할 듯
        if (context.phase == InputActionPhase.Performed)
        {
            currentMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
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
            isRunning= false;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            _animator.SetTrigger("Jump");
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            
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

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnCameraToggle(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            isFirstPerson = !isFirstPerson; // 모드 전환

            fpCamera.gameObject.SetActive(isFirstPerson);
            tpCamera.gameObject.SetActive(!isFirstPerson);
            crossHair.gameObject.SetActive(isFirstPerson);
        }
    }
}
