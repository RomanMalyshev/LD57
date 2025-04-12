using UnityEngine;
using static Globals;

public class RotationHandle : MonoBehaviour
{
    [SerializeField] private float minAngle = -45f;
    [SerializeField] private float maxAngle = 45f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private RotationHandleType _rotationHandleType;
    [SerializeField] private Camera Camera;
    private float currentAngle = 0f;

    // Fields for drag rotation logic
    private bool isDragging = false;
    private float initialAngle; // Object's angle when drag started (-180 to 180)
    private float initialMouseAngle; // Mouse angle when drag started
    private Vector3 screenPos; // Pivot's screen position
    private Camera mainCamera; // Cached camera
    private float previousMouseAngle; // Store mouse angle from previous frame

    void Start()
    {
        // Cache the camera, fallback to main camera if not assigned
        if (Camera == null)
        {
            Camera = Camera.main;
        }

        mainCamera = Camera;

        // Initialize current angle (optional, based on initial setup needs)
        currentAngle = Mathf.Clamp(transform.localEulerAngles.z > 180 ? transform.localEulerAngles.z - 360f : transform.localEulerAngles.z, minAngle, maxAngle);
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    public void Init()
    {
        currentAngle = minAngle;
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
        G.Presenter.OnFocusSet.Subscribe(SetFocus);
    }

    private void Update()
    {
        // Skip scroll wheel logic if dragging
        if (isDragging) return;

        if (G.Presenter.PlayerState.Value != GameStates.ResearcObject && _rotationHandleType == RotationHandleType.Focus) return;
        if (IsMouseOverHandle())
        {
            float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

            if (mouseWheelInput != 0f)
            {
                currentAngle -= mouseWheelInput * rotationSpeed;
                currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
                transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);

                UpdateValueBasedOnAngle();
            }
        }
    }

    private void OnMouseDown()
    {
        if (G.Presenter.PlayerState.Value != GameStates.ResearcObject && _rotationHandleType == RotationHandleType.Focus) return;

        if (mainCamera == null)
        {
            Debug.LogError("RotationHandle: Camera is not assigned or main camera not found.");
            return;
        }

        // Determine the pivot point screen position. Using transform.position for now.
        // Change to RotatePoint.position if that's the intended pivot.
        Vector3 pivotPosition = transform.position; // Or RotatePoint.position;
        screenPos = mainCamera.WorldToScreenPoint(pivotPosition);

        // Record the initial Z angle, normalized to -180 to 180 for consistency with Atan2
        float currentZ = transform.localEulerAngles.z;
        initialAngle = (currentZ > 180f) ? currentZ - 360f : currentZ;

        // Calculate the initial mouse angle relative to the screen position
        Vector3 mousePos = Input.mousePosition;
        Vector3 offset = mousePos - screenPos;
        initialMouseAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        isDragging = true; // Start dragging
        previousMouseAngle = initialMouseAngle; // Initialize previous angle
    }

    private void OnMouseDrag()
    {
        if (G.Presenter.PlayerState.Value != GameStates.ResearcObject && _rotationHandleType == RotationHandleType.Focus) return;

        if (!isDragging || mainCamera == null) return;

        Vector3 mousePos = Input.mousePosition;
        // Use the same screenPos calculated in OnMouseDown
        Vector3 offset = mousePos - screenPos;

        // Calculate current angle based on mouse position
        float currentMouseAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        
        // Determine rotation direction based on frame-to-frame change
        float frameDeltaAngle = Mathf.DeltaAngle(previousMouseAngle, currentMouseAngle);

        currentAngle += frameDeltaAngle * 0.3f;
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
        
        previousMouseAngle = currentMouseAngle;
        
        UpdateValueBasedOnAngle();
    }

    void OnMouseUp()
    {
        // Stop dragging when mouse button is released
        isDragging = false;
    }

    bool IsMouseOverHandle()
    {
        if (mainCamera == null) return false; // Use cached camera
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Ensure the raycast only hits this object's collider
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit collider belongs to this specific handle instance
            return hit.transform == transform;
        }

        return false;
    }

    private void SetFocus(float value)
    {
        if (_rotationHandleType == RotationHandleType.Focus)
        {
            currentAngle = Mathf.Lerp(minAngle, maxAngle, value);
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
            transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
    }

    // Method to update presenter value based on currentAngle
    private void UpdateValueBasedOnAngle()
    {
        float normalizedValue = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);
        switch (_rotationHandleType)
        {
            case RotationHandleType.Focus:
                G.Presenter.OnFocusChange.Value = normalizedValue;
                break;
            case RotationHandleType.Zoom:
                G.Presenter.OnZoom?.Invoke(normalizedValue);
                break;
            case RotationHandleType.GameVolume:
                G.Presenter.OnGameVolumeChange?.Invoke(normalizedValue);
                break;
            case RotationHandleType.Frequency:
                G.Presenter.OnFrequencyChenge?.Invoke(normalizedValue);
                break;
        }
    }
}

enum RotationHandleType
{
    GameVolume,
    Frequency,
    Focus,
    Zoom
}