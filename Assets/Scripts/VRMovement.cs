using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class VRMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float lookSpeed = 50.0f;

    private CharacterController characterController;
    private PlayerInputAction inputActions;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new PlayerInputAction();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable() => inputActions.Enable();

    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;

        Vector3 direction = (forward * moveInput.y + right * moveInput.x).normalized;
        characterController.Move(direction * moveSpeed * Time.deltaTime);
    }

    private void Look()
    {
        transform.Rotate(Vector3.up, lookInput.x * lookSpeed * Time.deltaTime);

        Transform vrCamera = Camera.main.transform;
        if (vrCamera != null)
        {
            float cameraPitch = vrCamera.localEulerAngles.x - lookInput.y * lookSpeed * Time.deltaTime;
            cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
            vrCamera.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        }
    }
}