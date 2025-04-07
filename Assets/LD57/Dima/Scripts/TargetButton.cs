using UnityEngine;
using static Globals;

public class TargetButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        G.Presenter.OnStartResearch?.Invoke();
    }
}
