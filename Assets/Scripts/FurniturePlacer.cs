using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class FurniturePlacer : MonoBehaviour
{
    public Transform player;
    public float placementDistance = 2f;
    public Material ghostMaterial;
    public Material failMaterial;
    public InputActionReference placeFurnitureAction;
    public InputActionReference rotateFurnitureAction;

    private LayerMask groundLayer;
    private LayerMask furnitureLayer;
    private LayerMask wallLayer;

    private Furniture selectedFurniture;
    private GameObject ghostObject;
    private Collider ghostCollider;
    private bool canPlace = true;
    private Quaternion ghostRotation = Quaternion.identity;

    private void OnEnable()
    {
        placeFurnitureAction.action.performed += OnPlaceFurniture;
        placeFurnitureAction.action.Enable();

        rotateFurnitureAction.action.performed += OnRotateFurniture;
        rotateFurnitureAction.action.Enable();
    }

    private void OnDisable()
    {
        placeFurnitureAction.action.performed -= OnPlaceFurniture;
        placeFurnitureAction.action.Disable();

        rotateFurnitureAction.action.performed -= OnRotateFurniture;
        rotateFurnitureAction.action.Disable();
    }

    public void SetSelectedFurniture(Furniture furniture)
    {
        selectedFurniture = furniture;
        groundLayer = selectedFurniture.groundLayer;
        furnitureLayer = LayerMask.GetMask("Furniture");
        wallLayer = LayerMask.GetMask("Wall");

        ghostRotation = Quaternion.identity;
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

        if (ghostCollider != null)
        {
            ghostCollider.enabled = false;
        }

        SetGhostAppearance(ghostObject);

        if (ghostCollider != null)
        {
            StartCoroutine(RecalculateCollider());
        }
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

        RaycastHit wallHit;
        if (Physics.Raycast(player.position, player.forward, out wallHit, placementDistance, wallLayer))
        {
            placementPosition = wallHit.point;
            placementPosition += wallHit.normal * 0.1f;

            if (wallHit.collider.bounds.size.y > 0)
            {
                placementPosition.y = wallHit.collider.bounds.center.y;
            }
        }
        else
        {
            placementPosition.y = GetGroundHeight(placementPosition);
        }

        RaycastHit roofHit;
        if (Physics.Raycast(placementPosition + Vector3.up * 2f, Vector3.down, out roofHit, 4f, LayerMask.GetMask("Roof")))
        {
            placementPosition.y = roofHit.point.y - 0.1f;
        }

        ghostObject.transform.position = placementPosition;
        ghostObject.transform.rotation = ghostRotation;

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
        if (!canPlace) 
        {
            SoundManager.PlaySound(SoundType.Forbid, volume: 1f);
            return;
        }

        PlaceFurniture(ghostObject.transform.position, ghostRotation);
    }

    private void PlaceFurniture(Vector3 position, Quaternion rotation)
    {
        GameObject placedObject = Instantiate(selectedFurniture.gameObject, position, rotation);

        Collider placedCollider = placedObject.GetComponent<Collider>();
        if (placedCollider != null)
        {
            placedCollider.enabled = false;
            StartCoroutine(EnableColliderAfterFrame(placedCollider));
        }

        SoundManager.PlaySound(SoundType.Build, volume: 1f);
        if (GlobalVariables.gameMode != "FreeBuild") 
        { 
            GlobalVariables.playerMoney -= selectedFurniture.Price;
            SoundManager.PlaySound(SoundType.Bought, volume: 0.8f);
        }

        Destroy(ghostObject);
        selectedFurniture = null;
    }

    private IEnumerator EnableColliderAfterFrame(Collider collider)
    {
        yield return new WaitForEndOfFrame();
        collider.enabled = true;
    }

    private void OnRotateFurniture(InputAction.CallbackContext context)
    {
        if (ghostObject == null) return;

        ghostRotation *= Quaternion.Euler(0, 90, 0);
        ghostObject.transform.rotation = ghostRotation;

        if (ghostCollider != null)
        {
            StartCoroutine(RecalculateCollider());
        }
    }

    private IEnumerator RecalculateCollider()
    {
        yield return null;

        if (ghostCollider is BoxCollider boxCollider)
        {
            Bounds bounds = new Bounds(ghostObject.transform.position, Vector3.zero);
            foreach (Renderer renderer in ghostObject.GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            }

            boxCollider.size = bounds.size;
            boxCollider.center = ghostObject.transform.InverseTransformPoint(bounds.center);
        }

        ghostCollider.enabled = true;
    }
}