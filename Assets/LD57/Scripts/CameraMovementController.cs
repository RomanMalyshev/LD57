using UnityEngine;
using static Globals;

public class CameraMovementController : MonoBehaviour
{
    [Header("References")]
    public Camera Camera;

    public Transform CameraRoot;

    [Header("Settings")]
    [SerializeField] private float _rotationSpeed = 50f;

    [SerializeField] private float _rotationSmoothTime = 0.1f;
    [SerializeField] private float _minPitch = -89f;
    [SerializeField] private float _maxPitch = 89f;
    [SerializeField] private float _stopThreshold = 2f; // Angle/FOV difference threshold to consider movement stopped

    // Target state
    private float _targetPitch;
    private float _targetYaw;
  
    // Current smoothed state
    private float _currentPitch;
    private float _currentYaw;
  
    // Smoothing velocities
    private float _pitchVelocity;
    private float _yawVelocity;
   
    // State flags for external use (e.g., audio)
    public bool IsApplyingRotationInput { get; private set; }
    public bool IsRotationSmoothing { get; private set; }

    private const float INPUT_ACTIVE_THRESHOLD = 0.01f; 

    public void Init()
    {

        // Initialize current rotation based on initial scene setup
        _currentPitch = Camera.transform.localEulerAngles.x;
        if (_currentPitch > 180) _currentPitch -= 360f; // Normalize pitch
        _currentYaw = CameraRoot.eulerAngles.y;

        // Initialize targets to match current state
        _targetPitch = _currentPitch;
        _targetYaw = _currentYaw;

      
        IsApplyingRotationInput = false;
        IsRotationSmoothing = false;
         }

    // Called externally (e.g., by Space class) when move input is received
    public void HandleMoveInput(Vector2 direction)
    {
        IsApplyingRotationInput = direction.magnitude > INPUT_ACTIVE_THRESHOLD;

        if (IsApplyingRotationInput)
        {
            float deltaTimeMultiplier = Time.deltaTime;
            _targetPitch += direction.y * _rotationSpeed * deltaTimeMultiplier;
            _targetPitch = Rom.MathHelper.ClampAngle(_targetPitch, _minPitch, _maxPitch);
            _targetYaw += direction.x * _rotationSpeed * deltaTimeMultiplier;
            // Yaw wraps automatically
        }
    }


    private void LateUpdate()
    {
        _currentPitch = Mathf.SmoothDampAngle(_currentPitch, _targetPitch, ref _pitchVelocity, _rotationSmoothTime);
        _currentYaw = Mathf.SmoothDampAngle(_currentYaw, _targetYaw, ref _yawVelocity, _rotationSmoothTime);
      
        // --- Apply Rotation and FOV ---
        Camera.transform.localRotation = Quaternion.Euler(-_currentPitch, 0.0f, 0.0f);
        CameraRoot.rotation = Quaternion.Euler(0.0f, _currentYaw, 0.0f);
         // --- Update Smoothing State Flags ---
        bool pitchSettled = Mathf.Abs(Mathf.DeltaAngle(_currentPitch, _targetPitch)) < _stopThreshold;
        bool yawSettled = Mathf.Abs(Mathf.DeltaAngle(_currentYaw, _targetYaw)) < _stopThreshold;
      
        // Rotation is smoothing if input was applied OR if it hasn't settled yet
        IsRotationSmoothing = !pitchSettled || !yawSettled;
    }
}