using UnityEngine;
using UnityEngine.UI;
using static Globals;

public class LokatorPanel : MonoBehaviour
{
    [SerializeField] private GameObject _lokator;

    public void Init()
    {
        G.Presenter.OnLocation.Subscribe(LocatorAnimation);
    }

    private void LocatorAnimation(float distanceToTarget)
    {
        
    }
}
