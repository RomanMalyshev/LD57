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
            Vector3 targetWorldPos = currentMouseWorldPos + screenPointOffset;

            Vector3 targetLocalPos = transform.parent == null ? targetWorldPos : transform.parent.InverseTransformPoint(targetWorldPos);

            float clampedLocalX = Mathf.Clamp(targetLocalPos.x, _minX, _maxX);

            transform.localPosition = new Vector3(clampedLocalX, transform.localPosition.y, transform.localPosition.z);

            _value = Mathf.InverseLerp(_minX, _maxX, clampedLocalX);
            _value = Mathf.Lerp(_minValue, _maxValue, _value);

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
                float targetPositionX = (value - _minValue) / (_maxValue - _minValue) * (_maxX - _minX) + _minX;
                transform.localPosition = new Vector3(targetPositionX, transform.localPosition.y, transform.localPosition.z);
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