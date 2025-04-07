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
            _detectedObject = spaceObject;
            IsZoomSmoothing = false;
            if(spaceObject == null)return;

            _currentScale =  _detectedObject.Sprite.transform.localScale;
        });
    }

    private void HandleZoomInput(float zoom)
    {
        if (_detectedObject == null) return;
        
        var spaceObject = _detector.CurrentlyDetectedObject;
        float targetScaleValue = Rom.MathHelper.Map(zoom, 0f, 1f, spaceObject._defaultScale, spaceObject._maxScale);
        _targetScale = Vector3.one * targetScaleValue;
        Debug.Log(_targetScale);
    }

    private void Update()
    {
        if (G.Presenter.PlayerState.Value != GameStates.Researching || _detectedObject == null) return;

        _currentScale = Vector3.SmoothDamp(_currentScale, _targetScale, ref _scaleVelocity, _scaleSmoothTime);
   
       // _detector.CurrentlyDetectedObject.Sprite.transform.localScale = _currentScale;
        bool scaleSettled = Vector3.Distance(_currentScale, _targetScale) < _stopThreshold;

        IsZoomSmoothing = !scaleSettled;
    }
}