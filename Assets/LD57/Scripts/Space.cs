using System;
using System.Collections;
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

    public CanvasGroup Aim;

    private CameraMovementController _movementController;
    private CameraAudioController _audioController;
    private CameraDetector _detector;
    private bool _firstEnter = true;
    
    public void Init()
    {
        Aim.alpha = 0f;
        _movementController = GetComponent<CameraMovementController>();
        _audioController = GetComponent<CameraAudioController>();
        _detector = GetComponent<CameraDetector>();

        _movementController.Camera = Camera;
        _movementController.CameraRoot = CameraRoot;

        _detector.TargetCamera = Camera;

        _movementController.Init();
        _spaceObjects.Init();

        G.Presenter.PlayerState.Subscribe(state =>
        {
            //StopAllCoroutines();
            StartCoroutine(SetAimState(state == GameStates.Exploring || state == GameStates.ResearcObject));
        });
        
        
    }

    private IEnumerator SetAimState(bool state)
    {
        var target = state ? 1f : 0f;
        var current = Aim.alpha;
        var elapsedTime = 0f;
        var duration = _firstEnter ? 3f:1.5f;
        if (_firstEnter)
        {
            yield return new WaitForSeconds(2f);
        }

        _firstEnter = false;
        while (elapsedTime <= duration)
        {
            Aim.alpha = Mathf.Lerp(current,target ,elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Aim.alpha = target;
    }
}