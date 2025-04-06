using System;
using UnityEngine;
using static Globals;

public class Space : MonoBehaviour
{

    public Camera Camera;
    public Transform CameraRoot;
    private float _cameraTargetPitch;
    private float _cameraTargetYaw;
    [SerializeField] private float _rotationSpeed = 50f; // Added default value
    [SerializeField] private float _rotationSmoothTime = 0.1f; // Renamed from _smoothTime
    [SerializeField] private float _zoomSmoothTime = 0.1f; // Added separate smooth time for zoom

    // Fields for smoothing rotation
    private float _currentPitch;
    private float _currentYaw;
    private float _pitchVelocity; // Ref variable for SmoothDampAngle
    private float _yawVelocity;   // Ref variable for SmoothDampAngle
    
    // Fields for smoothing FOV
    private float _targetFov;
    private float _currentFov;
    private float _fovVelocity; // Ref variable for SmoothDamp


    public void Init()
    {
        // Initialize current rotation values
        _currentPitch = Camera.transform.localEulerAngles.x;
        if (_currentPitch > 180) _currentPitch -= 360f; 
        _currentYaw = CameraRoot.eulerAngles.y;
        
        // Initialize target rotations to match current
        _cameraTargetPitch = _currentPitch;
        _cameraTargetYaw = _currentYaw;
        
        // Initialize FOV values
        _currentFov = Camera.fieldOfView;
        _targetFov = _currentFov;

        G.Presenter.OnMove.Subscribe(HandleMove);
        G.Presenter.OnZoom.Subscribe(HandleZoom);
    }

    private void HandleMove(Vector2 direction)
    {
        float deltaTimeMultiplier = Time.deltaTime; 

        // Update target pitch based on vertical input
        _cameraTargetPitch += direction.y * _rotationSpeed * deltaTimeMultiplier;
        _cameraTargetPitch = Rom.MathHelper.ClampAngle(_cameraTargetPitch, -89f, 89f); 

        // Update target yaw based on horizontal input
        _cameraTargetYaw += direction.x * _rotationSpeed * deltaTimeMultiplier;
    }

    private void HandleZoom(float zoom)
    {
        Debug.Log($"Zoom value: {zoom}"); // Slightly more informative log
        // Update target FOV based on zoom input
        _targetFov = Rom.MathHelper.Map(zoom, 0f, 1f, 60f, 30f); // Update target, not current
    }
    
    // Apply smoothed rotation and FOV in LateUpdate
    private void LateUpdate()
    {
        // Smoothly interpolate current pitch towards the target pitch
        _currentPitch = Mathf.SmoothDampAngle(_currentPitch, _cameraTargetPitch, ref _pitchVelocity, _rotationSmoothTime);

        // Smoothly interpolate current yaw towards the target yaw
        _currentYaw = Mathf.SmoothDampAngle(_currentYaw, _cameraTargetYaw, ref _yawVelocity, _rotationSmoothTime);
        
        // Smoothly interpolate current FOV towards the target FOV
        _currentFov = Mathf.SmoothDamp(_currentFov, _targetFov, ref _fovVelocity, _zoomSmoothTime);

        // Apply the smoothed rotations
        Camera.transform.localRotation = Quaternion.Euler(-_currentPitch, 0.0f, 0.0f); 
        CameraRoot.rotation = Quaternion.Euler(0.0f, _currentYaw, 0.0f); 
        
        // Apply the smoothed FOV
        Camera.fieldOfView = _currentFov;
    }

   
}
