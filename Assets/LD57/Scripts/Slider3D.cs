using UnityEngine;

namespace LD57.Scripts
{
    public class Slider3D:MonoBehaviour
    {
        private Vector3 screenPointOffset;
        private float initialZ; 
        
        private void Start()
        {
             // Store the initial distance from the camera for consistent dragging plane
            initialZ = Camera.main.WorldToScreenPoint(transform.position).z;
        }
        
        public void OnMouseDown()
        {
            Debug.Log("MouseDown");
            // Calculate the offset between the object's world position and the mouse's world position projected onto the object's plane
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = initialZ; // Use the stored Z depth
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            screenPointOffset = transform.position - mouseWorldPos;
        }

        public void OnMouseDrag()
        {
            // Get current mouse position in screen coordinates
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = initialZ; // Use the stored Z depth

            // Convert mouse position to world coordinates on the object's original plane
            Vector3 currentMouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            // Calculate the target position by applying the initial offset
            Vector3 targetPos = currentMouseWorldPos + screenPointOffset;

            // Constrain movement to the X-axis (relative to world space)
            // Keep the original Y and Z coordinates
            transform.position = new Vector3(targetPos.x, transform.position.y, transform.position.z);
        }

    }
}