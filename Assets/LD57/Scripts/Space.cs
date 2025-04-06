using System;
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

    [SerializeField] private SpaceObjects _spaceObjects;
    [Tooltip("The root transform for camera yaw rotation.")]
    public Transform CameraRoot;

    private CameraMovementController _movementController;
    private CameraAudioController _audioController;
    private CameraDetector _detector;

    private void Awake()
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
        _spaceObjects.Init();
        G.Presenter.OnMove.Subscribe(_movementController.HandleMoveInput);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ( G.Presenter.ResearchState.Value )
            {
                if (_detector.CurrentlyDetectedObject == null)
                {
                    Debug.Log("Can't send data CurrentlyDetectedObject is null");
                }
                else
                {
                    G.Presenter.ResearchState.Value = false;
                    Debug.Log("End Research, Data Sended");
                }

            }
            else
            {
                if (_detector.CurrentlyDetectedObject == null)
                {
                    Debug.Log("Nothing do detect  CurrentlyDetectedObject is null");
                }
                else
                {

                    G.Presenter.ResearchState.Value = true;
                    Debug.Log("Start Research");
                }  
                
            }
        }
    }
}
