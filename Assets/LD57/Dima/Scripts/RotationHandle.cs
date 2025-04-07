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

    public void Init()
    {
        currentAngle = minAngle;
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    void Update()
    {
        if (IsMouseOverHandle())
        {
            float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

            if (mouseWheelInput != 0f)
            {
                currentAngle -= mouseWheelInput * rotationSpeed;
                currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
                transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
                
                float normalizedValue = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);
                Debug.Log(normalizedValue +  " - Normilized value");
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
    }

    bool IsMouseOverHandle()
    {
        if (Camera == null) return false;
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform;
        }
        return false;
    }
}

enum RotationHandleType
{
    GameVolume,
    Frequency,
    Focus,
    Zoom
}
