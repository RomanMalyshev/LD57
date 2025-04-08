using System.Collections;
using Unity.VisualScripting;
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
        G.Presenter.OnFocusChange.Subscribe(focus =>
        {
            if (_detectedObject == null) return;
            _detectedObject.SetFocus(focus);
        });

        G.Presenter.DetectedObject.Subscribe(spaceObject =>
        {
            if (spaceObject == null) return;
            _detectedObject = spaceObject;
            IsZoomSmoothing = false;

            _currentScale = _detectedObject.Sprite.transform.localScale;
            _targetScale = _currentScale;
        });
    }

    private void HandleZoomInput(float zoom)
    {
        if (_detectedObject == null) return;
        _detectedObject.SetZoom(zoom);
        float targetScaleValue = Rom.MathHelper.Map(zoom, 0f, 1f, _detectedObject._defaultScale, _detectedObject._maxScale);
        _targetScale = Vector3.one * targetScaleValue;
    }

    private void Update()
    {
        if (_detectedObject == null)
        {
            G.Presenter.ResearchProgress.Value =0;
            return;
        }
        _currentScale = Vector3.SmoothDamp(_currentScale, _targetScale, ref _scaleVelocity, _scaleSmoothTime);

        _detectedObject.Sprite.transform.localScale = _currentScale;
        bool scaleSettled = Vector3.Distance(_currentScale, _targetScale) < _stopThreshold;
        
        IsZoomSmoothing = !scaleSettled;
        CalculateObjectProgress();
    }

    private void CalculateObjectProgress()
    {
        var focusComponent =
                G.Presenter.OnFocusChange.Value > _detectedObject.TargetFocus ? 
                   (1f- (G.Presenter.OnFocusChange.Value -  _detectedObject.TargetFocus) / _detectedObject.TargetFocus ):
                    G.Presenter.OnFocusChange.Value / _detectedObject.TargetFocus ;
        var zoomComponent =
            _currentScale.x > _detectedObject.TargetZoom ? 
                (1f - (_currentScale.x -  _detectedObject.TargetZoom) /  (_detectedObject.TargetZoom- _detectedObject._defaultScale)):
                (_currentScale.x - _detectedObject._defaultScale) / (_detectedObject.TargetZoom- _detectedObject._defaultScale); 
        
         G.Presenter.ResearchProgress.Value =
            (focusComponent + zoomComponent)/2 * 100 ;
    }
}