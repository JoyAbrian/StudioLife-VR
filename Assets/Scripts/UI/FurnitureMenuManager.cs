using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureMenuManager : MonoBehaviour
{
    [System.Serializable]
    public class FurnitureCategory
    {
        public string Name;
        public List<Furniture> FurnitureItems; // List of furniture ScriptableObjects for this category
    }

    [Header("Categories")]
    public List<FurnitureCategory> Categories;

    [Header("UI Elements")]
    public Transform FurnitureContent;
    public GameObject FurniturePrefabDisplay; // Prefab for the UI representation of furniture
    public Button[] SubmenuButtons;

    private void Start()
    {
        // Add listeners to submenu buttons
        for (int i = 0; i < SubmenuButtons.Length; i++)
        {
            int index = i; // Local copy of index to avoid closure issue
            SubmenuButtons[i].onClick.AddListener(() => ShowFurnitureMenu(index));
        }

        // Show the default menu (e.g., Living Room)
        ShowFurnitureMenu(0);
    }

    public void ShowFurnitureMenu(int categoryIndex)
    {
        // Clear existing items
        foreach (Transform child in FurnitureContent)
        {
            Destroy(child.gameObject);
        }

        // Get the selected category
        if (categoryIndex < 0 || categoryIndex >= Categories.Count) return;
        var selectedCategory = Categories[categoryIndex];

        // Populate the Scroll View with furniture items
        foreach (var furniture in selectedCategory.FurnitureItems)
        {
            GameObject item = Instantiate(FurniturePrefabDisplay, FurnitureContent);
            FurnitureDisplay display = item.GetComponent<FurnitureDisplay>();
            display.SetFurniture(furniture);

            // Optionally: Add click functionality to spawn or select furniture
            Button itemButton = item.GetComponent<Button>();
            if (itemButton != null)
            {
                itemButton.onClick.AddListener(() => SelectFurniture(furniture));
            }
        }
    }

    private void SelectFurniture(Furniture furniture)
    {
        Debug.Log($"Selected furniture: {furniture.FurnitureName} - ${furniture.Price:F2}");
        // Implement logic to spawn or apply furniture in the scene
    }
}