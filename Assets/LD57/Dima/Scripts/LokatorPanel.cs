using UnityEngine;
using static Globals;

public class LokatorPanel : MonoBehaviour
{
    [SerializeField] private GameObject _lokator;
    [SerializeField] private float _rotationSpeed = 5f; // Speed of the smooth rotation

    private Quaternion _targetRotation; // Target rotation to smoothly move towards

    public void Init()
    {
        if (_lokator != null)
        {
            _targetRotation = _lokator.transform.localRotation; // Initialize with current rotation
        }
        else
        {
             _targetRotation = Quaternion.identity;
            Debug.LogError("Lokator GameObject is not assigned in LokatorPanel.", this);
        }
        G.Presenter.DetectedObjectPower.Subscribe(LocatorAnimation);
    }

    private void Update()
    {
        if (_lokator != null)
        {
            // Smoothly interpolate the locator's rotation towards the target rotation
            _lokator.transform.localRotation = Quaternion.Slerp(
                _lokator.transform.localRotation,
                _targetRotation,
                Time.deltaTime * _rotationSpeed
            );
        }
    }

    private void LocatorAnimation(float distanceToTarget)
    {
       if (_lokator != null)
       {
            // Clamp the distance to the expected range [0, 100]
            float clampedDistance = Mathf.Clamp(distanceToTarget, 0f, 100f);

            // Map the distance [0, 100] to a Z rotation angle [0, 85] degrees
            float targetZAngle = (clampedDistance / 100f) * 85f;

            // Set the target rotation based on the calculated Z angle
            _targetRotation = Quaternion.Euler(0f, 0f, targetZAngle);
       }
    }
}
