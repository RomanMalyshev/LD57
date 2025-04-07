using UnityEngine;

public class Test : MonoBehaviour
{
    // Ограничения для движения по оси Y
    public float minY = -5000f;
    public float maxY = 5000f;

    private float offsetY;

    void OnMouseDown()
    {
        // Рассчитываем смещение, чтобы объект не скачал при начале перетаскивания
        offsetY = transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
    }

    void OnMouseDrag()
    {
        // Получаем позицию мыши в мировых координатах
        float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y + offsetY;

        // Ограничиваем движение объекта в пределах minY и maxY
        mouseY = Mathf.Clamp(mouseY, minY, maxY);

        // Обновляем позицию объекта по оси Y
        transform.position = new Vector3(transform.position.x, mouseY, transform.position.z);
    }
}
