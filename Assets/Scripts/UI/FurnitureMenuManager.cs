using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureMenuManager : MonoBehaviour
{
    public FurniturePlacer FurniturePlacer;
    [System.Serializable]
    public class FurnitureCategory
    {
        public string Name;
        public List<Furniture> FurnitureItems;
    }

    [Header("Categories")]
    public List<FurnitureCategory> Categories;

    [Header("UI Elements")]
    public Transform FurnitureContent;
    public GameObject FurniturePrefabDisplay;
    public Button[] SubmenuButtons;

    private void Start()
    {
        for (int i = 0; i < SubmenuButtons.Length; i++)
        {
            int index = i;
            SubmenuButtons[i].onClick.AddListener(() => ShowFurnitureMenu(index));
        }

        ShowFurnitureMenu(0);
    }

    public void ShowFurnitureMenu(int categoryIndex)
    {
        foreach (Transform child in FurnitureContent)
        {
            Destroy(child.gameObject);
        }

        if (categoryIndex < 0 || categoryIndex >= Categories.Count) return;
        var selectedCategory = Categories[categoryIndex];

        foreach (var furniture in selectedCategory.FurnitureItems)
        {
            GameObject item = Instantiate(FurniturePrefabDisplay, FurnitureContent);
            FurnitureDisplay display = item.GetComponent<FurnitureDisplay>();
            display.SetFurniture(furniture);

            Button itemButton = item.GetComponent<Button>();
            if (itemButton != null)
            {
                itemButton.onClick.AddListener(() => SelectFurniture(furniture));
            }
        }
    }

    private void SelectFurniture(Furniture furniture)
    {
        MenuManager.CloseAllMenus();
        FurniturePlacer.SetSelectedFurniture(furniture);
    }
}