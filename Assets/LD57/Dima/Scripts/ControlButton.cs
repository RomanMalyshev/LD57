using UnityEngine;
using static Globals;

public class ControlButton : MonoBehaviour
{
    [SerializeField] private ButtonDirection _direction;
    private Vector2 direction;
    
    private void OnMouseDown()
    {
        switch (_direction)
        {
            case ButtonDirection.Up:
                direction = Vector2.up;
                break;
            case ButtonDirection.Down:
                direction = Vector2.down;
                break;
            case ButtonDirection.Right:
                direction = Vector2.right;
                break;
            case ButtonDirection.Left:
                direction = Vector2.left;
                break;
        }
        
        G.Presenter.OnControlButtonDown?.Invoke(direction);
    }

    private void OnMouseUp()
    {
        G.Presenter.OnControlButtonUp?.Invoke();
    }
}


enum ButtonDirection
{
    Up,
    Down,
    Left,
    Right
}
