using System;
using UnityEngine;
using static Globals;

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
    public InSpaceObject CurrentlyDetectedObject { get; private set; }
    public bool IsDetectingTarget { get; private set; }
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
        if (CurrentlyDetectedObject == null || CurrentlyDetectedObject.Reserched)
        {
            G.Presenter.DetectedObjectPower.Value = 0;
        }
        else
        {
            Vector3 camPos = TargetCamera.transform.position;
            Vector3 camForward = TargetCamera.transform.forward;
            Vector3 objPos = CurrentlyDetectedObject.transform.position;

            // Calculate the perpendicular distance from object center to the camera ray
            Vector3 camToObj = objPos - camPos;
            Vector3 projection = Vector3.Project(camToObj, camForward);
            Vector3 closestPointOnRay = camPos + projection;
            float perpDistance = Vector3.Distance(objPos, closestPointOnRay);
            
            var normalizedValue = 1.0f - Mathf.Clamp01(perpDistance/7.6f);//sphere collider radius 
            // Scale to 0-100 range
            G.Presenter.DetectedObjectPower.Value = normalizedValue * 100.0f;
        }
    }

    private void PerformDetection()
    {
        Ray ray = new Ray(TargetCamera.transform.position, TargetCamera.transform.forward);
        RaycastHit hit;

        bool previouslyDetecting = IsDetectingTarget;
        InSpaceObject previousObject = CurrentlyDetectedObject;

        if (Physics.Raycast(ray, out hit, _raycastLength, _raycastLayerMask))
        {
            LastHitInfo = hit; // Store hit info regardless of tag
            if (!string.IsNullOrEmpty(_targetTag) && hit.collider.CompareTag(_targetTag))
            {
                IsDetectingTarget = true;

                if (hit.transform.TryGetComponent<InSpaceObject>(out InSpaceObject spaceObject) && !spaceObject.Reserched)
                {
                    CurrentlyDetectedObject = spaceObject;
                }

                if (!previouslyDetecting || previousObject != CurrentlyDetectedObject)
                {
                    G.Presenter.DetectedObject.Value = spaceObject;
                    Debug.Log($"Detected Target '{_targetTag}': {hit.collider.name}", this);
                }
            }
            else
            {
                // Hit something, but not the target tag (or tag is empty)
                IsDetectingTarget = false;
                CurrentlyDetectedObject = null;
                G.Presenter.DetectedObject.Value = null;
            }
        }
        else
        {
            // Hit nothing
            IsDetectingTarget = false;
            CurrentlyDetectedObject = null;
            // Clear LastHitInfo if needed, depending on desired behavior when hitting nothing
            // LastHitInfo = default; 
            G.Presenter.DetectedObject.Value = null;
        } //

        if (previouslyDetecting && !IsDetectingTarget && previousObject != null)
        {
            Debug.Log($"Stopped detecting Target '{_targetTag}': {previousObject.name}", this);
        }
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
        else
        {
            gizmoColor = Color.red; // Hitting nothing or not detecting the target tag
        }

        Gizmos.color = gizmoColor;
        Gizmos.DrawRay(ray.origin, ray.direction * drawLength);
    }
}