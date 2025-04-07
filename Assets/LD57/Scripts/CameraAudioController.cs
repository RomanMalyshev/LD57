using UnityEngine;

[RequireComponent(typeof(CameraMovementController))]
public class CameraAudioController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private LoopSoundSingleClip ZoomSound;
    [SerializeField] private LoopSoundSingleClip MoveSound;

    [Header("Input Thresholds (match Space/MovementController if needed)")]
    [SerializeField] private float ZoomActiveThreshold = 0.05f;
    [SerializeField] private float MoveActiveThreshold = 0.05f;

    [SerializeField] private CameraMovementController _movementController;
    [SerializeField] private SpaceObjects _spaceObjects;

    // Internal state to track if sounds are currently playing
    private bool _isZoomSoundPlaying = false;
    private bool _isMoveSoundPlaying = false;

    private void Awake()
    {
        _movementController = GetComponent<CameraMovementController>();
    }

    private void Update()
    {
        UpdateZoomSound();
        UpdateMoveSound();
    }

    private void UpdateZoomSound()
    {
        if (ZoomSound == null) return;

        // Start sound if zoom input is active OR smoothing is happening, and sound isn't already playing
        bool shouldPlayZoom = _spaceObjects.IsZoomSmoothing;
        if (shouldPlayZoom && !_isZoomSoundPlaying)
        {
             ZoomSound.StartLoop();
            _isZoomSoundPlaying = true;
        }
        // Stop sound if no zoom input AND smoothing has finished, and sound is currently playing
        else if (!shouldPlayZoom && _isZoomSoundPlaying)
        {
             ZoomSound.StopLoop();
            _isZoomSoundPlaying = false;
        }
    }

    private void UpdateMoveSound()
    {
        if (MoveSound == null) return;

        // Start sound if rotation input is active OR smoothing is happening, and sound isn't already playing
        bool shouldPlayMove =  _movementController.IsRotationSmoothing;

        if (shouldPlayMove && !_isMoveSoundPlaying)
        {
            MoveSound.StartLoop();
            _isMoveSoundPlaying = true;
        }
        // Stop sound if no rotation input AND smoothing has finished, and sound is currently playing
        else if (!shouldPlayMove && _isMoveSoundPlaying)
        {
            MoveSound.StopLoop();
            _isMoveSoundPlaying = false;
        }
    }
} 