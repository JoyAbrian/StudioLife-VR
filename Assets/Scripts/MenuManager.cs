using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public GameObject FurnitureMenu;
    public InputActionReference ToggleMenuAction;

    private bool isMenuVisible = false;

    private void OnEnable()
    {
        ToggleMenuAction.action.performed += OnToggleMenu;
    }

    private void OnDisable()
    {
        ToggleMenuAction.action.performed -= OnToggleMenu;
    }

    private void OnToggleMenu(InputAction.CallbackContext context)
    {
        isMenuVisible = !isMenuVisible;
        FurnitureMenu.SetActive(isMenuVisible);

        if (isMenuVisible)
        {
            Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
            FurnitureMenu.transform.position = targetPosition;

            FurnitureMenu.transform.LookAt(Camera.main.transform);
            FurnitureMenu.transform.Rotate(0, 180, 0);
        }
    }
}