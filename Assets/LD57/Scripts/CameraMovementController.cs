using LD57.Scripts;
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
    [SerializeField] private float _zoomSmoothTime = 0.1f;
    [SerializeField] private float _minFov = 55f;
    [SerializeField] private float _maxFov = 60f;
   
    [SerializeField] private float _stopThreshold = 2f;

    // Target state
    private float _targetPitch;
    private float _targetYaw;
    private float _targetFov;

    // Current smoothed state
    private float _currentPitch;
    private float _currentYaw;
    private float _currentFov;

    // Smoothing velocities
    private float _pitchVelocity;
    private float _yawVelocity;
    private float _fovVelocity;

    // State flags for external use (e.g., audio)
    public bool IsApplyingRotationInput { get; private set; }
    public bool IsApplyingZoomInput { get; private set; } 
    public bool IsRotationSmoothing { get; private set; }
    public bool IsZoomSmoothing { get; private set; } 

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

        // Initialize FOV
        _currentFov = Camera.fieldOfView;
        _targetFov = _currentFov;

        IsApplyingRotationInput = false;
        IsApplyingZoomInput = false;
        IsRotationSmoothing = false;
        IsZoomSmoothing = false;
        
        G.Presenter.OnZoom.Subscribe(HandleZoomInput);
        
    }

    // Called externally (e.g., by Space class) when move input is received
    public void HandleMoveInput(Vector2 direction)
    {
        IsApplyingRotationInput = direction.magnitude > INPUT_ACTIVE_THRESHOLD;

        if (IsApplyingRotationInput)
        {
            float deltaTimeMultiplier = Time.deltaTime;
            _targetPitch += direction.y * _rotationSpeed * deltaTimeMultiplier;
            _targetPitch = Rom.MathHelper.ClampAngle(_targetPitch, GamePreferences.MIN_PITCH, GamePreferences.MAX_PITCH);
            _targetYaw += direction.x * _rotationSpeed * deltaTimeMultiplier;
            _targetYaw = Rom.MathHelper.ClampAngle(_targetYaw, GamePreferences.MIN_YOW, GamePreferences.MAX_YOW);
          
            

        }
    }

    // Called externally when zoom input is received
    public void HandleZoomInput(float zoom) // zoom is expected to be 0-1
    {
        IsApplyingZoomInput = zoom > INPUT_ACTIVE_THRESHOLD;

        if (IsApplyingZoomInput)
        {
            // Map 0-1 zoom input to FOV range
            _targetFov = Rom.MathHelper.Map(zoom, 0f, 1f, _maxFov, _minFov); // Reversed mapping: 1 = min FOV (zoomed in)
            _targetFov = Mathf.Clamp(_targetFov, _minFov, _maxFov);
        }
    }

    private void LateUpdate()
    {
        _currentPitch = Mathf.SmoothDampAngle(_currentPitch, _targetPitch, ref _pitchVelocity, _rotationSmoothTime);
        _currentYaw = Mathf.SmoothDampAngle(_currentYaw, _targetYaw, ref _yawVelocity, _rotationSmoothTime);
        _currentFov = Mathf.SmoothDamp(_currentFov, _targetFov, ref _fovVelocity, _zoomSmoothTime);

        // --- Apply Rotation and FOV ---
        Camera.transform.localRotation = Quaternion.Euler(-_currentPitch, 0.0f, 0.0f);
        CameraRoot.rotation = Quaternion.Euler(0.0f, _currentYaw, 0.0f);
        G.Presenter.TelescopeRotation.Invoke(new Vector2(_currentPitch,_currentYaw));
        Camera.fieldOfView = _currentFov;

        // --- Update Smoothing State Flags ---
        bool pitchSettled = Mathf.Abs(Mathf.DeltaAngle(_currentPitch, _targetPitch)) < _stopThreshold;
        bool yawSettled = Mathf.Abs(Mathf.DeltaAngle(_currentYaw, _targetYaw)) < _stopThreshold;
        bool fovSettled = Mathf.Abs(_currentFov - _targetFov) < _stopThreshold;

        // Rotation is smoothing if input was applied OR if it hasn't settled yet
        IsRotationSmoothing = !pitchSettled || !yawSettled;
        IsZoomSmoothing =  !fovSettled;
        
    }
}