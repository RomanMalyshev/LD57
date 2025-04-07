using UnityEngine;
using static Globals;

public class StartButton : MonoBehaviour
{
    private bool _isGameIsTarted;

    public void Init()
    {
        _isGameIsTarted = false;
    }
    private void OnMouseDown()
    {
        if (!_isGameIsTarted)
            G.Presenter.OnGameStarted?.Invoke(1);
        
        _isGameIsTarted = !_isGameIsTarted;
    }
}
