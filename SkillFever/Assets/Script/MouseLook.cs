using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    //マウスの感度設定
    [SerializeField] float mouseSensitivity = 100f;

    private InputSystem_Actions inputActions;
    private Vector2 lookInput;

    float xRotation = 0f;

    public Transform playerBody;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.Player.Look.performed += ctx =>
        {
            lookInput = ctx.ReadValue<Vector2>();
        };

        inputActions.Player.Look.canceled += ctx =>
        {
            lookInput = Vector2.zero;
        };
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}