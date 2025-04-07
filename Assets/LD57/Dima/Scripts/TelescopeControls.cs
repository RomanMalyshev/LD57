using UnityEngine;
using static Globals;

public class TelescopeControls : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private TargetButton _targetButton;
    
    private Vector2 _wASDdirection;
    private Vector2 _buttonsDirection;
    private Vector2 _targetPosition;
    private Vector2 _direction;
    private bool _isControlButtonDown;
    private GameStates _gamestate;
    public void Init()
    {
        
        
        G.Presenter.OnTargetAreaEnter.Subscribe(TargetPositionSet);
        G.Presenter.OnControlButtonDown.Subscribe(ControlButtonDown);
        G.Presenter.OnControlButtonUp.Subscribe(ControlButtonUp);
        G.Presenter.OnTargetButtonDown.Subscribe(SetTarget);
        G.Presenter.PlayerState.Subscribe(SetState);
    }

    private void Update()
    {
        return;
        if (_gamestate == GameStates.Exploring)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                _wASDdirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }

            if (_isControlButtonDown)
            {
                _direction = (_wASDdirection + _buttonsDirection).normalized * Time.deltaTime * _speed;
            }
            else
            {
                _direction = _wASDdirection.normalized * Time.deltaTime * _speed;
            }

            if (_isControlButtonDown || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                G.Presenter.OnMove?.Invoke(_direction);
            }
        }
    }

    private void TargetPositionSet(Vector2 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void SetTarget(bool isOnTarget)
    {
        if (isOnTarget)
            G.Presenter.OnTargetAcquisition?.Invoke(_targetPosition);
        else
            G.Presenter.OnZoom?.Invoke(0);
    }
    
    public void ControlButtonDown(Vector2 direction)
    {
        _isControlButtonDown = true;
        _buttonsDirection = direction;
    }

    public void ControlButtonUp()
    {
        _isControlButtonDown = false;
    }
    
    private void SetState(GameStates state)
    {
        _gamestate = state;
    }
}


