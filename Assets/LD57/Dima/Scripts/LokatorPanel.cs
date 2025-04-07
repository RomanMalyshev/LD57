using UnityEngine;
using UnityEngine.UI;
using static Globals;

public class LokatorPanel : MonoBehaviour
{
    [SerializeField] private Image _lokator;

    public void Init()
    {
        G.Presenter.OnLocation.Subscribe(LocatorAnimation);
    }

    private void LocatorAnimation(float distanceToTarget)
    {
        
    }
}
