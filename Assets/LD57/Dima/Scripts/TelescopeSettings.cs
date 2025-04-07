using LD57.Scripts;
using UnityEngine;
using static Globals;

public class TelescopeSettings : MonoBehaviour
{
    [SerializeField] private Slider3D _zoomSlider;
    [SerializeField] private GameObject _lokator;
    [SerializeField] private RotationHandle _focus;
    [SerializeField] private EndReaserchButton _endReaserchButton;
    [SerializeField] private float minValue = 0f;  
    [SerializeField] private float maxValue = 100f; 
    [SerializeField] private float minAngle = -90f; 
    [SerializeField] private float maxAngle = 90f; 
    
    private float _zoom;
    private bool _isOpticFiterOn = false;
    private bool _isRadioFiterOn = false;
    private bool _isInfraredFiterOn = false;
    private bool _isUVFiterOn = false;
    private bool _isReserchIsDone = false;
    private GameStates _gamestate;
    
    
    public void Init()
    {
        _zoomSlider.Init();
        _focus.Init();
        G.Presenter.OnFilterButtonClick.Subscribe(SetFilters);
        G.Presenter.OnLocation.Subscribe(LocatorAnimation);
        G.Presenter.PlayerState.Subscribe(SetState);
    }
    void SetFilters(FiltersType filtersType)
    {
        if (_gamestate == GameStates.ResearcObject)
        {
            switch (filtersType)
            {
                case FiltersType.Optic:
                    _isOpticFiterOn = !_isOpticFiterOn;
                    Debug.Log("Filter "+filtersType + "" + _isOpticFiterOn);
                    G.Presenter.OnFilterSet?.Invoke(FiltersType.Optic, _isOpticFiterOn);
                    break;
                case FiltersType.Radio:
                    _isRadioFiterOn = !_isRadioFiterOn;
                    Debug.Log("Filter "+filtersType + "" + _isRadioFiterOn);
                    G.Presenter.OnFilterSet?.Invoke(FiltersType.Radio, _isRadioFiterOn);
                    break;
                case FiltersType.Infrared:
                    _isInfraredFiterOn = !_isInfraredFiterOn;
                    Debug.Log("Filter "+filtersType + "" + _isInfraredFiterOn);
                    G.Presenter.OnFilterSet?.Invoke(FiltersType.Infrared, _isInfraredFiterOn);
                    break;
                case FiltersType.UV:
                    _isUVFiterOn = !_isUVFiterOn;
                    Debug.Log("Filter "+filtersType + "" + _isUVFiterOn);
                    G.Presenter.OnFilterSet?.Invoke(FiltersType.UV, _isUVFiterOn);
                    break;
            }
        }
    }
    
    private void LocatorAnimation(float distanceToTarget)
    {
        distanceToTarget = Mathf.Clamp(distanceToTarget, minValue, maxValue);
        
        float angle = Mathf.Lerp(minAngle, maxAngle, (distanceToTarget - minValue) / (maxValue - minValue));
        
        _lokator.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
    
    private void SetState(GameStates state)
    {
        _gamestate = state;
    }
}

public enum FiltersType
{
    Optic,
    Radio,
    Infrared,
    UV
}