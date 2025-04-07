using UnityEngine;
using static Globals;

namespace LD57.Scripts
{
    public class Slider3D : MonoBehaviour
    {
        [SerializeField] SliderType _sliderType;
        [SerializeField] float _minX = -3f;
        [SerializeField] float _maxX = 3f;
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private float _maxValue = 1f;
        private Vector3 screenPointOffset;
        private float initialZ;
        private float _value;
        private GameStates _gamestate;

        public void Init()
        {
            G.Presenter.OnZoomSet.Subscribe(SetZoom);
            initialZ = Camera.main.WorldToScreenPoint(transform.position).z;
            G.Presenter.PlayerState.Subscribe(SetState);
        }

        private void SetState(GameStates state)
        {
            _gamestate = state;
        }

        public void OnMouseDown()
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = initialZ;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            screenPointOffset = transform.position - mouseWorldPos;
        }

        public void OnMouseDrag()
        {
            if (_gamestate != GameStates.ResearcObject && _sliderType == SliderType.Zoom) return;

            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = initialZ;

            Vector3 currentMouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            Vector3 targetPos = currentMouseWorldPos + screenPointOffset;

            Vector3 localTargetPosition = transform.InverseTransformPoint(targetPos);

            if ((localTargetPosition.x + transform.localPosition.x) > _minX && (localTargetPosition.x + transform.localPosition.x) < _maxX)
            {
                transform.position = new Vector3(targetPos.x, transform.position.y, transform.position.z);
            }

            _value = (transform.localPosition.x - _minX) / (_maxX - _minX) * (_maxValue - _minValue) + _minValue;

            switch (_sliderType)
            {
                case SliderType.Zoom:
                    G.Presenter.OnZoom?.Invoke(_value);
                    break;
                case SliderType.GameVolume:
                    G.Presenter.OnGameVolumeChange?.Invoke(_value);
                    break;
                case SliderType.Frequency:
                    G.Presenter.OnFrequencyChenge?.Invoke(_value);
                    break;
            }
        }

        private void SetZoom(float value)
        {
            if (_sliderType == SliderType.Zoom)
            {
                float tragetPositionX = (value - _minValue) / (_maxValue - _minValue) * (_maxX - _minX) + _minX;
                transform.position = new Vector3(tragetPositionX, transform.position.y, transform.position.z);
            }
        }
    }
}

enum SliderType
{
    GameVolume,
    Zoom,
    Frequency
}