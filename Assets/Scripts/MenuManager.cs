using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("Furniture Menu")]
    public GameObject FurnitureMenu;
    public InputActionReference ToggleOpenFurnitureMenu;

    [Header("Texture Menu")]
    public GameObject TextureMenu;
    public InputActionReference ToggleOpenTextureMenu;

    private bool isFurnitureMenuVisible = false;
    private bool isTextureMenuVisible = false;

    private void OnEnable()
    {
        ToggleOpenFurnitureMenu.action.performed += ToggleFurnitureMenu;
        ToggleOpenTextureMenu.action.performed += ToggleTextureMenu;
    }

    private void OnDisable()
    {
        ToggleOpenFurnitureMenu.action.performed -= ToggleFurnitureMenu;
        ToggleOpenTextureMenu.action.performed -= ToggleTextureMenu;
    }

    private void ToggleFurnitureMenu(InputAction.CallbackContext context)
    {
        if (isFurnitureMenuVisible)
        {
            isFurnitureMenuVisible = false;
            UpdateMenuVisibility(FurnitureMenu, isFurnitureMenuVisible);
        }
        else
        {
            isTextureMenuVisible = false;
            UpdateMenuVisibility(TextureMenu, isTextureMenuVisible);

            isFurnitureMenuVisible = true;
            UpdateMenuVisibility(FurnitureMenu, isFurnitureMenuVisible);
        }
    }

    private void ToggleTextureMenu(InputAction.CallbackContext context)
    {
        if (isTextureMenuVisible)
        {
            isTextureMenuVisible = false;
            UpdateMenuVisibility(TextureMenu, isTextureMenuVisible);
        }
        else
        {
            isFurnitureMenuVisible = false;
            UpdateMenuVisibility(FurnitureMenu, isFurnitureMenuVisible);

            isTextureMenuVisible = true;
            UpdateMenuVisibility(TextureMenu, isTextureMenuVisible);
        }
    }

    private void UpdateMenuVisibility(GameObject menu, bool isVisible)
    {
        if (menu == null) return;

        menu.SetActive(isVisible);

        if (isVisible)
        {
            Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
            menu.transform.position = targetPosition;

            menu.transform.LookAt(Camera.main.transform);
            menu.transform.Rotate(0, 180, 0);
        }
    }
}