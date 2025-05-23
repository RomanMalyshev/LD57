using UnityEngine;
using static Globals;

public class FilterButton : MonoBehaviour
{
    [SerializeField] private FiltersType _filtersType;
    private bool _isFilterOn;
    
    private void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        G.Presenter.OnFilterButtonClick?.Invoke(_filtersType);
    }
}
