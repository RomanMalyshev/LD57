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
        G.Presenter.OnZoomSet.Subscribe(SetZoom);
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

    private void SetZoom(float value)
    {
        if (_rotationHandleType == RotationHandleType.Zoom)
        {
            currentAngle = Mathf.Lerp(minAngle, maxAngle, value);
            transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
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
