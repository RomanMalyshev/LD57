using UnityEngine;
using UnityEngine.UI;
using static Globals;

public class TelescopeControls : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Vector2 _wASDdirection;
    private Vector2 _buttonsDirection;
    private Vector2 _targetPosition;
    private Vector2 _direction;
    private bool _isControlButtonDown;
    public void Init()
    {
        //_targetAcquisitionButton.onClick.AddListener(() =>SetTarget());
        G.Presenter.OnTargetAreaEnter.Subscribe(TargetPositionSet);
        G.Presenter.OnControlButtonDown.Subscribe(ControlButtonDown);
        G.Presenter.OnControlButtonUp.Subscribe(ControlButtonUp);
    }

    private void Update()
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
            //Debug.Log(_direction);
        }
    }

    private void TargetPositionSet(Vector2 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void SetTarget()
    {
        G.Presenter.OnTargetAcquisition?.Invoke(_targetPosition);
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
}


