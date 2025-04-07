using UnityEngine;
using static Globals;

public class TargetButton : MonoBehaviour
{
    private bool _isOnTarget;

    public void Init()
    {
        _isOnTarget = false;
    }
    private void OnMouseDown()
    {
        _isOnTarget = !_isOnTarget;
        G.Presenter.OnTargetButtonDown?.Invoke(_isOnTarget);
    }
}
