using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class FurniturePlacer : MonoBehaviour
{
    public Transform player;
    public float placementDistance = 2f;
    public Material ghostMaterial;
    public Material failMaterial;
    public InputActionReference placeFurnitureAction;

    private LayerMask groundLayer;
    private LayerMask furnitureLayer;
    private LayerMask wallLayer;

    private Furniture selectedFurniture;
    private GameObject ghostObject;
    private Collider ghostCollider;
    private bool canPlace = true;

    private void OnEnable()
    {
        placeFurnitureAction.action.performed += OnPlaceFurniture;
        placeFurnitureAction.action.Enable();
    }

    private void OnDisable()
    {
        placeFurnitureAction.action.performed -= OnPlaceFurniture;
        placeFurnitureAction.action.Disable();
    }

    public void SetSelectedFurniture(Furniture furniture)
    {
        selectedFurniture = furniture;
        groundLayer = selectedFurniture.groundLayer;
        furnitureLayer = LayerMask.GetMask("Furniture");
        wallLayer = LayerMask.GetMask("Wall");

        CreateGhost();
    }

    private void CreateGhost()
    {
        if (ghostObject != null)
        {
            Destroy(ghostObject);
        }

        if (selectedFurniture == null) return;

        ghostObject = Instantiate(selectedFurniture.gameObject);
        ghostCollider = ghostObject.GetComponent<Collider>();
        SetGhostAppearance(ghostObject);
    }

    private void SetGhostAppearance(GameObject ghost)
    {
        foreach (var renderer in ghost.GetComponentsInChildren<Renderer>())
        {
            renderer.material = new Material(ghostMaterial);
            Color color = renderer.material.color;
            color.a = 0.5f;
            renderer.material.color = color;
        }
    }

    private void Update()
    {
        if (selectedFurniture == null || ghostObject == null) return;

        Vector3 placementPosition = player.position + player.forward * placementDistance;
        placementPosition.y = GetGroundHeight(placementPosition);
        ghostObject.transform.position = placementPosition;

        canPlace = !IsColliding(ghostObject);
        UpdateGhostMaterial(canPlace);
    }

    private float GetGroundHeight(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 5, Vector3.down, out hit, 10f, groundLayer))
        {
            return hit.point.y;
        }
        return position.y;
    }

    private bool IsColliding(GameObject ghost)
    {
        if (ghostCollider == null) return false;

        LayerMask collisionMask = furnitureLayer | wallLayer;

        Collider[] colliders = Physics.OverlapBox(
            ghostCollider.bounds.center,
            ghostCollider.bounds.extents,
            ghostObject.transform.rotation,
            collisionMask
        );

        foreach (var col in colliders)
        {
            if (col.gameObject != ghost && col.gameObject != player.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateGhostMaterial(bool valid)
    {
        foreach (var renderer in ghostObject.GetComponentsInChildren<Renderer>())
        {
            renderer.material = valid ? ghostMaterial : failMaterial;
            Color color = renderer.material.color;
            color.a = 0.5f;
            renderer.material.color = color;
        }
    }

    private void OnPlaceFurniture(InputAction.CallbackContext context)
    {
        if (selectedFurniture == null || ghostObject == null || EventSystem.current.IsPointerOverGameObject()) return;
        if (!canPlace) return;

        PlaceFurniture(ghostObject.transform.position);
    }

    private void PlaceFurniture(Vector3 position)
    {
        GameObject placedObject = Instantiate(selectedFurniture.gameObject, position, Quaternion.identity);
        Destroy(ghostObject);
        selectedFurniture = null;
    }
}