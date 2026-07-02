using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float Speed = 3f;
    [SerializeField] float JumpPower = 5f;
    [SerializeField] Transform cameraTransform;

    [SerializeField] float fallMultiplier = 2f;

    private Rigidbody rb;
    private InputSystem_Actions inputAction;
    private Vector2 moveInput;
    private bool isGrounded = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                   RigidbodyConstraints.FreezeRotationZ;

        inputAction = new InputSystem_Actions();

        inputAction.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };

        inputAction.Player.Move.canceled += ctx =>
        {
            moveInput = Vector2.zero;
        };
        inputAction.Player.Jump.performed += ctx =>
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
                isGrounded = false;
            }
        };
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;

        rb.linearVelocity = new Vector3(moveDirection.x * Speed, rb.linearVelocity.y, moveDirection.z * Speed);

        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Physics.gravity * (fallMultiplier - 1), ForceMode.Acceleration);
        }

    }
    
}