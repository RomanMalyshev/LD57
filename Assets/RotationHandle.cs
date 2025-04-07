using UnityEngine;
using static Globals;

public class RotationHandle : MonoBehaviour
{
    // Минимальный и максимальный углы вращения (в градусах)
    [SerializeField] private float minAngle = -45f;
    [SerializeField] private  float maxAngle = 45f;

    [SerializeField] private RotationHandleType _rotationHandleType;

    // Текущее значение угла вращения
    private float currentAngle = 0f;

    // Степень чувствительности прокрутки колесика мыши
    public float rotationSpeed = 5f;

    void Start()
    {
        // Инициализируем угол вращения
        currentAngle = minAngle;
        // Устанавливаем начальную ориентацию ручки
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    void Update()
    {
        // Проверяем, находится ли курсор над ручкой
        if (IsMouseOverHandle())
        {
            // Получаем ввод от колесика мыши
            float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

            // Если прокручиваем колесо мыши
            if (mouseWheelInput != 0f)
            {
                // Изменяем текущий угол в зависимости от прокрутки колесика
                currentAngle -= mouseWheelInput * rotationSpeed;

                // Ограничиваем угол вращения в заданных пределах
                currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

                // Применяем новый угол вращения к ручке вокруг локальной оси Z
                transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);

                // Вычисляем значение от 0 до 1 в зависимости от угла
                float normalizedValue = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);

                // Логируем текущее значение (0 или 1) на основе угла
                //Debug.Log("Current Value: " + normalizedValue);
                switch (_rotationHandleType)
                {
                    case RotationHandleType.Focus:
                        G.Presenter.OnFocusChange?.Invoke(normalizedValue);
                        break;
                    case RotationHandleType.Zoom:
                        G.Presenter.OnZoom?.Invoke(normalizedValue);
                        break;
                    case RotationHandleType.GameVolume:
                        G.Presenter.OnGameVolumeChange?.Invoke(normalizedValue);
                        break;
                    case RotationHandleType.Frequency:
                        G.Presenter.OnFrequencyChenge?.Invoke(normalizedValue);
                        break;
                }
            }
        }
    }

    // Проверка, находится ли курсор мыши над ручкой
    bool IsMouseOverHandle()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Если курсор мыши попадает в коллайдер ручки
            return hit.transform == transform;
        }

        return false;
    }
}

enum RotationHandleType
{
    GameVolume,
    Frequency,
    Focus,
    Zoom
}
