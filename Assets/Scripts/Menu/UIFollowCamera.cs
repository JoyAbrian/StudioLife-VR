using UnityEngine;
using UnityEngine.InputSystem;

public class UIFollowCamera : MonoBehaviour
{
    public Transform vrCamera;
    public float distance = 2.0f;
    public float heightOffset = 0.0f;
    public InputActionReference repositionAction;

    private void Start()
    {
        MoveToFrontOfPlayer();
    }

    private void OnEnable()
    {
        if (repositionAction != null)
        {
            repositionAction.action.performed += OnRepositionAction;
            repositionAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (repositionAction != null)
        {
            repositionAction.action.performed -= OnRepositionAction;
            repositionAction.action.Disable();
        }
    }

    private void OnRepositionAction(InputAction.CallbackContext context)
    {
        MoveToFrontOfPlayer();
    }

    private void MoveToFrontOfPlayer()
    {
        if (vrCamera == null) return;

        transform.position = vrCamera.position + vrCamera.forward * distance + Vector3.up * heightOffset;
        transform.rotation = Quaternion.LookRotation(transform.position - vrCamera.position);
    }
}
