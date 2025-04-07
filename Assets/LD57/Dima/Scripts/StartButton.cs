using UnityEngine;
using static Globals;

public class StartButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        G.Presenter.OnStartGame?.Invoke();
    }
}
