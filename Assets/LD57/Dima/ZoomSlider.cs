using System;
using UnityEngine.EventSystems; // Для интерфейсов
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; // Для работы с UI

public class ZoomSlider : MonoBehaviour//, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
   // Минимальное и максимальное значения слайдера
    public float minValue = 0f;
    public float maxValue = 100f;

    // Ссылки на объекты слайдера
    public Transform sliderBackground; // Фон слайдера
    public Transform sliderHandle; // Ползунок слайдера

    private bool isDragging = false;
    private Vector3 initialMousePos; // Начальная позиция мыши в мировых координатах
    private Vector3 initialHandlePos; // Начальная позиция ползунка в локальных координатах

    private float currentValue;

    void Start()
    {
        // Устанавливаем начальное значение слайдера
        SetSliderValue((minValue + maxValue) / 2f);

        // Логируем информацию о компонентах
        if (sliderBackground.GetComponent<Collider>() == null)
        {
            Debug.LogError("Background does not have a collider!");
        }
        else
        {
            Debug.Log("Background collider found.");
        }

        if (sliderHandle.GetComponent<Collider>() == null)
        {
            Debug.LogError("Handle does not have a collider!");
        }
        else
        {
            Debug.Log("Handle collider found.");
        }
    }

    // Этот метод будет вызываться при клике на ползунок
    private void OnMouseDown()
    {
        // Проверяем, был ли клик на ползунке
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == sliderHandle) // Проверка, что мы кликаем на ползунок
            {
                Debug.Log("Mouse down on slider handle");

                isDragging = true;

                // Получаем начальную позицию мыши в мировых координатах
                initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                initialMousePos.z = 0; // Игнорируем ось Z

                // Сохраняем начальную позицию ползунка в локальных координатах
                initialHandlePos = sliderHandle.localPosition;
            }
        }
    }

    // Этот метод будет вызываться при перемещении мыши с зажатой кнопкой
    private void OnMouseDrag()
    {
        if (isDragging)
        {
            // Получаем текущую позицию мыши в мировых координатах
            Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePos.z = 0; // Игнорируем ось Z

            // Вычисляем разницу между текущей и начальной позицией мыши по оси X
            float mouseDeltaX = currentMousePos.x - initialMousePos.x;

            // Логируем движение мыши
            Debug.Log("Mouse dragging. DeltaX: " + mouseDeltaX);

            // Обновляем позицию ползунка с учетом разницы
            Vector3 newHandlePos = initialHandlePos + new Vector3(mouseDeltaX, 0f, 0f);

            // Ограничиваем движение ползунка в пределах фона слайдера по оси X
            float minX = -sliderBackground.localScale.x / 2f;
            float maxX = sliderBackground.localScale.x / 2f;
            newHandlePos.x = Mathf.Clamp(newHandlePos.x, minX, maxX);

            // Устанавливаем новую позицию ползунка
            sliderHandle.localPosition = new Vector3(newHandlePos.x, sliderHandle.localPosition.y, sliderHandle.localPosition.z);

            // Обновляем текущее значение слайдера в зависимости от положения ползунка
            currentValue = Mathf.InverseLerp(minX, maxX, newHandlePos.x) * (maxValue - minValue) + minValue;

            // Логируем текущее значение слайдера
            Debug.Log("Slider value: " + currentValue);
        }
    }

    // Этот метод будет вызываться, когда пользователь отпустит кнопку мыши
    private void OnMouseUp()
    {
        if (isDragging)
        {
            Debug.Log("Mouse up on slider handle");
            isDragging = false;
        }
    }

    // Метод для программной установки значения слайдера
    public void SetSliderValue(float value)
    {
        currentValue = Mathf.Clamp(value, minValue, maxValue);

        // Переводим значение слайдера в локальную позицию ползунка
        float normalizedValue = Mathf.InverseLerp(minValue, maxValue, currentValue);
        float newLocalX = Mathf.Lerp(-sliderBackground.localScale.x / 2f, sliderBackground.localScale.x / 2f, normalizedValue);

        sliderHandle.localPosition = new Vector3(newLocalX, sliderHandle.localPosition.y, sliderHandle.localPosition.z);

        // Логируем установку значения слайдера
        Debug.Log("Slider value set to: " + currentValue);
    }

    // Получение текущего значения слайдера
    public float GetSliderValue()
    {
        return currentValue;
    }
}
