using UnityEngine;
using static Globals;

[RequireComponent(typeof(CameraMovementController))]
[RequireComponent(typeof(CameraAudioController))]
[RequireComponent(typeof(CameraDetector))]
public class Space : MonoBehaviour
{
    [Header("Core References")]
    [Tooltip("The main camera being controlled.")]
    public Camera Camera;

    [Tooltip("The root transform for camera yaw rotation.")]
    public Transform CameraRoot;

    // References to the specialized controller components
    private CameraMovementController _movementController;
    private CameraAudioController _audioController;
    private CameraDetector _detector;

    void Awake()
    {
        _movementController = GetComponent<CameraMovementController>();
        _audioController = GetComponent<CameraAudioController>(); 
        _detector = GetComponent<CameraDetector>();

        _movementController.Camera = this.Camera;
        _movementController.CameraRoot = this.CameraRoot;

        _detector.TargetCamera = this.Camera;
    }

    public void Init()
    {
        _movementController.Init();
        G.Presenter.OnMove.Subscribe(_movementController.HandleMoveInput);
        G.Presenter.OnZoom.Subscribe(_movementController.HandleZoomInput);
    }
}