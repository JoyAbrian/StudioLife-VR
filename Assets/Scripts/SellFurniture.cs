using UnityEngine;
using UnityEngine.InputSystem;

public class SellFurniture : MonoBehaviour
{
    public Camera playerCamera;
    public InputActionReference sellFurnitureAction;
    public float sellRange = 5f;

    private void OnEnable()
    {
        sellFurnitureAction.action.performed += OnSellFurniture;
        sellFurnitureAction.action.Enable();
    }

    private void OnDisable()
    {
        sellFurnitureAction.action.performed -= OnSellFurniture;
        sellFurnitureAction.action.Disable();
    }

    private void OnSellFurniture(InputAction.CallbackContext context)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, sellRange))
        {
            Furniture furniture = hit.collider.GetComponent<Furniture>();

            if (furniture != null)
            {
                Sell(furniture);
            }
        }
    }

    private void Sell(Furniture furniture)
    {
        int refundAmount = Mathf.RoundToInt(furniture.Price);
        GlobalVariables.playerMoney += refundAmount;

        SoundManager.PlaySound(SoundType.Bought, volume: 1f);
        Destroy(furniture.gameObject);
    }
}
