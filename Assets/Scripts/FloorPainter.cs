using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloorPainter : MonoBehaviour
{
    public Material ghostMaterial;
    public InputActionReference acceptAction;

    private List<Renderer> floorRenderers;
    private List<Material> originalMaterials;
    private Paint selectedPaint;
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

    private void OnAcceptAction(UnityEngine.InputSystem.InputAction.CallbackContext context)
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

        var floorsInRoom = GameObject.FindGameObjectsWithTag("Floor");
        foreach (var floor in floorsInRoom)
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
        if (floorRenderers.Count > 0 && selectedPaint != null)
        {
            for (int i = 0; i < floorRenderers.Count; i++)
            {
                floorRenderers[i].material = selectedPaint.paintMaterial;
            }

            isPainting = false;
        }
    }
}