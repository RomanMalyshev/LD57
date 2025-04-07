using UnityEngine;
using static Globals;

public class SpaceObjects : MonoBehaviour
{
    [SerializeField] private CameraDetector _detector;
    private InSpaceObject[] _spaceObjects;

    private Vector3 _scaleVelocity;
    [SerializeField] private float _scaleSmoothTime = 3f;
    [SerializeField] private float _minScale = 0.5f;
    [SerializeField] private float _maxScale = 2.0f;
    private float _stopThreshold = 0.05f;

    private Vector3 _currentScale;
    private Vector3 _targetScale;
    public bool IsZoomSmoothing { get; private set; }
    private InSpaceObject _detectedObject;
    
    public void Init()
    {
        _spaceObjects = GetComponentsInChildren<InSpaceObject>();

        G.Presenter.OnZoom.Subscribe(HandleZoomInput);

        G.Presenter.DetectedObject.Subscribe(spaceObject =>
        {
            if(spaceObject == null)return;
            _detectedObject = spaceObject;
            IsZoomSmoothing = false;

            _currentScale =  _detectedObject.Sprite.transform.localScale;
            _targetScale = _currentScale;
        });
    }

    private void HandleZoomInput(float zoom)
    {
        if (_detectedObject == null) return;
        
        float targetScaleValue = Rom.MathHelper.Map(zoom, 0f, 1f, _detectedObject._defaultScale, _detectedObject._maxScale);
        _targetScale = Vector3.one * targetScaleValue;
    }

    private void Update()
    {
        if (_detectedObject == null) return;

        _currentScale = Vector3.SmoothDamp(_currentScale, _targetScale, ref _scaleVelocity, _scaleSmoothTime);
   
        _detectedObject.Sprite.transform.localScale = _currentScale;
        bool scaleSettled = Vector3.Distance(_currentScale, _targetScale) < _stopThreshold;

        IsZoomSmoothing = !scaleSettled;
    }
}