using UnityEngine;
using static Globals;

public class StartButton : MonoBehaviour
{
    public ButtonLamp Lamp;

    private void OnMouseDown()
    {
        G.Presenter.OnStartGame?.Invoke();
    }
}
