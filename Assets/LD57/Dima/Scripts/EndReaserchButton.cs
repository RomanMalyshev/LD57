using UnityEngine;
using static Globals;

public class EndReaserchButton : MonoBehaviour
{
    private GameStates _gamestate;
    
    public void Init()
    {
        G.Presenter.PlayerState.Subscribe(SetState);
    }

    private void SetState(GameStates state)
    {
        _gamestate = state;
    }
    private void OnMouseDown()
    {
        if (_gamestate == GameStates.ResearcObject)
            G.Presenter.OnSendData?.Invoke();
    }
}
