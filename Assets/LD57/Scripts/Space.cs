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

    public GameObject Aim;

    private CameraMovementController _movementController;
    private CameraAudioController _audioController;
    private CameraDetector _detector;

    public void Init()
    {
        _movementController = GetComponent<CameraMovementController>();
        _audioController = GetComponent<CameraAudioController>();
        _detector = GetComponent<CameraDetector>();

        _movementController.Camera = Camera;
        _movementController.CameraRoot = CameraRoot;

        _detector.TargetCamera = Camera;

        _movementController.Init();
        _spaceObjects.Init();

        G.Presenter.PlayerState.Subscribe(state => { Aim.gameObject.SetActive(state == GameStates.Exploring || state == GameStates.ResearcObject); });
    }
}