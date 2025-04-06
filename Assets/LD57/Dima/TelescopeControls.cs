using UnityEngine;
using UnityEngine.UI;
using static Globals;

public class TelescopeControls : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Button _up;
    [SerializeField] private Button _down;
    [SerializeField] private Button _right;
    [SerializeField] private Button _left;
    private Vector2 _wASDdirection;
    private Vector2 _buttonsDirection;
    private Vector2 _direction;
    private bool _isControlButtonDown;
    public void Init()
    {
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            _wASDdirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        if (_isControlButtonDown)
        {
            _direction = (_wASDdirection + _buttonsDirection).normalized * (_speed * Time.deltaTime);
        }
        else
        {
            _direction = _wASDdirection.normalized * (_speed * Time.deltaTime);
        }

        if (_isControlButtonDown || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            G.Presenter.OnMove?.Invoke(_direction);
        }
    }
    
    public void ControlButtonDown(string direction)
    {
        _isControlButtonDown = true;

        switch (direction)
        {
            case "up":
                _buttonsDirection = Vector2.up;
                Debug.Log("Up");
                break;
            case "down":
                _buttonsDirection = Vector2.down;
                Debug.Log("Down");
                break;
            case "right":
                _buttonsDirection = Vector2.right;
                Debug.Log("Right");
                break;
            case "left":
                _buttonsDirection = Vector2.left;
                Debug.Log("Left");
                break;
        } 
    }

    public void ControlButtonUp()
    {
        _isControlButtonDown = false;
    }
}

