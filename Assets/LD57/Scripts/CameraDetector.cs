using System;
using UnityEngine;

public class CameraDetector : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The camera to cast the ray from.")]
    public Camera TargetCamera;

    [Header("Detection Settings")]
    [SerializeField] private float _raycastLength = 100f; // Configurable raycast length

    [SerializeField] private LayerMask _raycastLayerMask = ~0; // Raycast against all layers by default
    [SerializeField] private string _targetTag = "SpaceObject"; // Tag to detect

    // Public properties to access detection results
    public Transform CurrentlyDetectedObject { get; private set; }
    public bool IsDetectingTarget { get; private set; }
    public Action<SpaceObjects> ObjectDetected;
    public RaycastHit LastHitInfo { get; private set; }

    private bool _hasWarnedMissingCamera = false;
    
    private void Awake()
    {
        if (TargetCamera == null)
        {
            Debug.LogWarning("TargetCamera is not assigned in CameraDetector. Attempting to find main camera.", this);
            TargetCamera = Camera.main;
        }
    }

    public void Update()
    {
        PerformDetection();
    }
    
    public void PerformDetection()
    {
        Ray ray = new Ray(TargetCamera.transform.position, TargetCamera.transform.forward);
        RaycastHit hit;

        bool previouslyDetecting = IsDetectingTarget;
        Transform previousObject = CurrentlyDetectedObject;

        if (Physics.Raycast(ray, out hit, _raycastLength, _raycastLayerMask))
        {
            LastHitInfo = hit; // Store hit info regardless of tag
            if (!string.IsNullOrEmpty(_targetTag) && hit.collider.CompareTag(_targetTag))
            {
                IsDetectingTarget = true;
                CurrentlyDetectedObject = hit.transform;
                if (!previouslyDetecting || previousObject != CurrentlyDetectedObject)
                {
                    Debug.Log($"Detected Target '{_targetTag}': {hit.collider.name}", this);
                }
            }
            else
            {
                // Hit something, but not the target tag (or tag is empty)
                IsDetectingTarget = false;
                CurrentlyDetectedObject = null;
            }
        }
     //  else
     //  {
     //      // Hit nothing
     //      IsDetectingTarget = false;
     //      CurrentlyDetectedObject = null;
     //      // Clear LastHitInfo if needed, depending on desired behavior when hitting nothing
     //      // LastHitInfo = default; 
     //  }//
     //  if (previouslyDetecting && !IsDetectingTarget && previousObject != null)
     //  {
     //      Debug.Log($"Stopped detecting Target '{_targetTag}': {previousObject.name}", this);
     //  }
    }

    // Draw Gizmos to visualize the detection ray
    private void OnDrawGizmos()
    {
        if (TargetCamera == null) return; // Don't draw if no camera

        Ray ray = new Ray(TargetCamera.transform.position, TargetCamera.transform.forward);
        float drawLength = _raycastLength;
        Color gizmoColor;

        // Check current state for coloring
        if (IsDetectingTarget && CurrentlyDetectedObject != null)
        {
            gizmoColor = Color.green; // Currently detecting the target
            // Use LastHitInfo if you want the gizmo to stop exactly at the hit point
            if (LastHitInfo.collider != null) // Check if LastHitInfo is valid
            {
                drawLength = LastHitInfo.distance;
            }
        }
        // Optional: Add different color if hitting *something* but not the target
        // else if (Physics.Raycast(ray, out RaycastHit anyHit, _raycastLength, _raycastLayerMask))
        // {
        //     gizmoColor = Color.yellow; // Hitting something else
        //     drawLength = anyHit.distance;
        // }
        else
        {
            gizmoColor = Color.red; // Hitting nothing or not detecting the target tag
        }

        Gizmos.color = gizmoColor;
        Gizmos.DrawRay(ray.origin, ray.direction * drawLength);
    }
}