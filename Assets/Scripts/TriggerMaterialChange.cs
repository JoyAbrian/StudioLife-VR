using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerMaterialChange : MonoBehaviour
{
    public Material newMaterial;  // The material to change to
    private Material originalMaterial;  // To store the original material
    private Renderer objectRenderer;  // Renderer of the object

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that triggered is from the XR controller or other desired object
        if (other.CompareTag("Player"))  // Example: Assuming the XR controller is tagged "Player"
        {
            objectRenderer.material = newMaterial;  // Change to the new material
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Revert back to the original material when exiting the trigger zone
        if (other.CompareTag("Player"))
        {
            objectRenderer.material = originalMaterial;  // Revert to the original material
        }
    }
}