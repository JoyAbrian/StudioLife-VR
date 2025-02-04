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

    [Header("Props Menu")]
    public GameObject PropsMenu;
    public InputActionReference ToggleOpenPropsMenu;

    public static bool isFurnitureMenuVisible = false;
    public static bool isTextureMenuVisible = false;
    public static bool isPropsMenuVisible = false;

    public static MenuManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        ToggleOpenFurnitureMenu.action.performed += ToggleFurnitureMenu;
        ToggleOpenTextureMenu.action.performed += ToggleTextureMenu;
        ToggleOpenPropsMenu.action.performed += TogglePropsMenu;
    }

    private void OnDisable()
    {
        ToggleOpenFurnitureMenu.action.performed -= ToggleFurnitureMenu;
        ToggleOpenTextureMenu.action.performed -= ToggleTextureMenu;
        ToggleOpenPropsMenu.action.performed -= TogglePropsMenu;
    }

    private void ToggleFurnitureMenu(InputAction.CallbackContext context)
    {
        SetMenuVisibility(FurnitureMenu, ref isFurnitureMenuVisible);
    }

    private void ToggleTextureMenu(InputAction.CallbackContext context)
    {
        SetMenuVisibility(TextureMenu, ref isTextureMenuVisible);
    }

    private void TogglePropsMenu(InputAction.CallbackContext context)
    {
        SetMenuVisibility(PropsMenu, ref isPropsMenuVisible);
    }

    private void SetMenuVisibility(GameObject menu, ref bool menuVisibility)
    {
        isFurnitureMenuVisible = false;
        isTextureMenuVisible = false;
        isPropsMenuVisible = false;

        UpdateMenuVisibility(FurnitureMenu, isFurnitureMenuVisible);
        UpdateMenuVisibility(TextureMenu, isTextureMenuVisible);
        UpdateMenuVisibility(PropsMenu, isPropsMenuVisible);

        menuVisibility = !menuVisibility;
        UpdateMenuVisibility(menu, menuVisibility);
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

    public static void CloseAllMenus()
    {
        if (Instance == null) return;

        isFurnitureMenuVisible = false;
        isTextureMenuVisible = false;
        isPropsMenuVisible = false;

        Instance.UpdateMenuVisibility(Instance.FurnitureMenu, false);
        Instance.UpdateMenuVisibility(Instance.TextureMenu, false);
        Instance.UpdateMenuVisibility(Instance.PropsMenu, false);
    }
}