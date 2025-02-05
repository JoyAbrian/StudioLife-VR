using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloorPainter : MonoBehaviour
{
    public Material ghostMaterial;
    public InputActionReference acceptAction;

    private List<Renderer> floorRenderers;
    private List<Material> originalMaterials;
    public static Paint selectedPaint;
    private bool isPainting = false;

    public Paint SelectedPaint => selectedPaint;

    private void OnEnable()
    {
        acceptAction.action.performed += OnAcceptAction;
        acceptAction.action.Enable();
    }

    private void OnDisable()
    {
        acceptAction.action.performed -= OnAcceptAction;
        acceptAction.action.Disable();
    }

    private void OnAcceptAction(InputAction.CallbackContext context)
    {
        if (isPainting && selectedPaint != null)
        {
            ApplySelectedPaint(selectedPaint);
        }
    }

    public void SelectFloorPaint(Paint paint)
    {
        if (paint.type != Paint.Type.Floor) return;

        selectedPaint = paint;

        floorRenderers = new List<Renderer>();
        originalMaterials = new List<Material>();

        if (GlobalVariables.selectedRoomFloor == null || GlobalVariables.selectedRoomFloor.Length == 0)
        {
            Debug.LogWarning("No floors selected in GlobalVariables.selectedRoomFloor!");
            return;
        }

        foreach (var floor in GlobalVariables.selectedRoomFloor)
        {
            var renderer = floor.GetComponent<Renderer>();
            if (renderer != null)
            {
                floorRenderers.Add(renderer);
                originalMaterials.Add(renderer.material);
                renderer.material = ghostMaterial;
            }
        }

        isPainting = true;
    }

    private void ApplySelectedPaint(Paint selectedPaint)
    {
        if (GlobalVariables.selectedRoomFloor == null || GlobalVariables.selectedRoomFloor.Length == 0) return;

        foreach (var floor in GlobalVariables.selectedRoomFloor)
        {
            var renderer = floor.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = selectedPaint.paintMaterial;
            }
        }

        isPainting = false;
    }
}
