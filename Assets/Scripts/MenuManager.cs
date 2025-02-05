using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI[] moneyTextList;

    [Header("Furniture Menu")]
    public GameObject FurnitureMenu;
    public InputActionReference ToggleOpenFurnitureMenu;

    [Header("Texture Menu")]
    public GameObject TextureMenu;
    public InputActionReference ToggleOpenTextureMenu;

    [Header("Props Menu")]
    public GameObject PropsMenu;
    public InputActionReference ToggleOpenPropsMenu;

    private GameObject currentOpenMenu = null;

    public static MenuManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GlobalVariables.gameMode == "Free Build")
        {
            return;
        }

        foreach (TextMeshProUGUI moneyText in moneyTextList)
        {
            moneyText.text = "Money : $" + GlobalVariables.playerMoney.ToString();
        }
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
        ToggleMenu(FurnitureMenu);
    }

    private void ToggleTextureMenu(InputAction.CallbackContext context)
    {
        ToggleMenu(TextureMenu);
    }

    private void TogglePropsMenu(InputAction.CallbackContext context)
    {
        ToggleMenu(PropsMenu);
    }

    private void ToggleMenu(GameObject menu)
    {
        if (menu == currentOpenMenu)
        {
            CloseAllMenus();
            return;
        }

        CloseAllMenus();

        currentOpenMenu = menu;
        menu.SetActive(true);

        Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
        menu.transform.position = targetPosition;
        menu.transform.LookAt(Camera.main.transform);
        menu.transform.Rotate(0, 180, 0);
    }

    public static void CloseAllMenus()
    {
        if (Instance == null) return;

        if (Instance.currentOpenMenu != null)
        {
            Instance.currentOpenMenu.SetActive(false);
            Instance.currentOpenMenu = null;
        }
    }
}