using System;
using UnityEngine;
using UnityEngine.Events; // Add this for UnityAction
using UnityEngine.EventSystems; // Add this for Event System interfaces

// Implement pointer interfaces
public class Rotator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private float rotationSpeed = 10f; // Speed of rotation
    public UnityAction<float> onValueChanged; // Action invoked with rotation delta around Z-axis

    private bool isRotating = false;
    private Vector2 previousMousePosition; // Use Vector2 for screen positions
    private float totalRotationZ = 0f; // Track total rotation around Z

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialization if needed
    }

    // Update is no longer needed for input checks
    void Update()
    {
        // Can be removed or used for other logic if needed
    }

    // Called when the pointer clicks down on the object
    public void OnPointerDown(PointerEventData eventData)
    {
        isRotating = true;
        previousMousePosition = eventData.position; // Use eventData's screen position
    }

    // Called when the pointer drags over the object
    public void OnDrag(PointerEventData eventData)
    {
        if (!isRotating) return; // Only rotate if dragging started on this object

        Vector2 currentMousePosition = eventData.position;
        Vector2 deltaMousePosition = currentMousePosition - previousMousePosition;

        // Calculate rotation amount for Z-axis based on horizontal mouse movement
        float rotationZ = -deltaMousePosition.x * rotationSpeed * Time.deltaTime; // Horizontal mouse -> Z-axis rotation

        // Apply rotation around world Z axis
        transform.Rotate(Vector3.forward, rotationZ, UnityEngine.Space.World);

        // Accumulate total rotation if needed
        totalRotationZ += rotationZ;

        // Invoke the action with the rotation delta for this frame
        onValueChanged?.Invoke(rotationZ);
        previousMousePosition = currentMousePosition; // Update position for next delta calculation
    }

    // Called when the pointer releases the click on the object (or moves off)
    public void OnPointerUp(PointerEventData eventData)
    {
        isRotating = false;
    }

    // Optional: Add a method to allow external scripts to subscribe easily
    public void AddListener(UnityAction<float> listener)
    {
        onValueChanged += listener;
    }

    // Optional: Remove listener
    public void RemoveListener(UnityAction<float> listener)
    {
        onValueChanged -= listener;
    }
}
