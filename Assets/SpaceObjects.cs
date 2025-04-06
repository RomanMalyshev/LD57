using System;
using Unity.VisualScripting;
using UnityEngine;
using static Globals;

public class SpaceObjects : MonoBehaviour
{
    [SerializeField] private CameraDetector _detector;
    private InSpaceObject[] _spaceObjects;

    public bool IsApplyingZoomInput { get; private set; }
    private float _zoomVelocity;
    private float _zoomSmoothTime = 0.1f;
   [SerializeField] private float _minDetectedObjectZ = 5f;
   [SerializeField] private float _maxDetectedObjectZ = 30f;
    private float _stopThreshold = 2f; // Angle/FOV difference threshold to consider movement stopped

    private float _currentDetectedObjectZ;
    private float _targetDetectedObjectZ;
    private const float INPUT_ACTIVE_THRESHOLD = 0.01f;
    public bool IsZoomSmoothing { get; private set; }

    public void Init()
    {
        _spaceObjects = GetComponentsInChildren<InSpaceObject>();

        G.Presenter.OnZoom.Subscribe(HandleZoomInput);

        _detector.ObjectDetected = spaceObject =>
        {
            IsApplyingZoomInput = false;
            IsZoomSmoothing = false;

            _currentDetectedObjectZ = spaceObject.transform.localPosition.z;
            _targetDetectedObjectZ = _currentDetectedObjectZ;
        };
    }

    // Called externally when zoom input is received
    private void HandleZoomInput(float zoom) // zoom is expected to be 0-1
    {
        if (_detector.CurrentlyDetectedObject == null) return;
        IsApplyingZoomInput = zoom > INPUT_ACTIVE_THRESHOLD;
        _zoomVelocity = 5;
        if (IsApplyingZoomInput)
        {
            // Map 0-1 zoom input to FOV range
            _targetDetectedObjectZ = Rom.MathHelper.Map(zoom, 0f, 1f, _maxDetectedObjectZ, _minDetectedObjectZ); // Reversed mapping: 1 = min FOV (zoomed in)
            _targetDetectedObjectZ = Mathf.Clamp(_targetDetectedObjectZ, _minDetectedObjectZ, _maxDetectedObjectZ);
            Debug.Log(_targetDetectedObjectZ);
        }
    }

    private void Update()
    {
        if (!G.Presenter.ResearchState.Value || _detector.CurrentlyDetectedObject == null) return;

        Transform detectedObjectTransform = _detector.CurrentlyDetectedObject;
        float zBeforeSmoothing = _currentDetectedObjectZ;

        _currentDetectedObjectZ = Mathf.SmoothDamp(_currentDetectedObjectZ, _targetDetectedObjectZ, ref _zoomVelocity, _zoomSmoothTime);

        float deltaZThisFrame = _currentDetectedObjectZ - zBeforeSmoothing;

        if (Mathf.Abs(deltaZThisFrame) > Mathf.Epsilon)
        {
            detectedObjectTransform.Translate(Vector3.forward * deltaZThisFrame, UnityEngine.Space.Self);
        }

        bool fovSettled = Mathf.Abs(_currentDetectedObjectZ - _targetDetectedObjectZ) < _stopThreshold;

        IsZoomSmoothing = !fovSettled;
    }
}