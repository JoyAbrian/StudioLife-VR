using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureDisplay : MonoBehaviour
{
    public Image IconImage;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI PriceText;

    public void SetFurniture(Furniture furniture)
    {
        IconImage.sprite = furniture.Icon;
        NameText.text = furniture.FurnitureName;
        PriceText.text = $"${furniture.Price:F2}";
    }
}