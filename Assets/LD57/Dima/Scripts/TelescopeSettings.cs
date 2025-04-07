using UnityEngine;
using UnityEngine.UI;
using static Globals;

public class TelescopeSettings : MonoBehaviour
{
    //[SerializeField] private Slider _zoomSlider;
    private float _zoom;
    private bool _isOpticFiterOn = false;
    private bool _isRadioFiterOn = false;
    private bool _isInfraredFiterOn = false;
    private bool _isUVFiterOn = false;
    private bool _isReserchIsDone = false;
    
    public void Init()
    {
        
        //TODO: так замыкать нельзя, спроси протом почему
       // G.Presenter.OnZoom.Subscribe(GetZoomValue);
        
        
        //_endReserchButton.onClick.AddListener(() => EndReserchButtonClicked());
        
        //G.Presenter.OnReserchIsDone.Subscribe(SetReserchStatus);
        G.Presenter.OnFilterButtonClick.Subscribe(SetFilters);
        //_zoomSlider.onValueChanged.AddListener(ZoomChange);
    }

    /*private void ZoomChange(float value)
    {
        _zoom = value;
        G.Presenter.OnZoom?.Invoke(_zoom);
    }*/

    void SetFilters(FiltersType filtersType)
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

    /*private void GetZoomValue(float zoom)
    {
        _zoomSlider.value = zoom;
    }*/

    /*public void ZoomChange()
    {
        _zoom = _zoomSlider.value;
        G.Presenter.OnZoom?.Invoke(_zoom);
    }*/

    /*private void SetReserchStatus(bool isReserchIsDone)
    {
        _isReserchIsDone = isReserchIsDone;
    }
    private void EndReserchButtonClicked()
    {
        G.Presenter.OnEndOfReserchButtonClicked?.Invoke(_isReserchIsDone);
    }*/
}

public enum FiltersType
{
    Optic,
    Radio,
    Infrared,
    UV
}