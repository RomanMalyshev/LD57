using System.Collections;
using UnityEngine;

public class LoopSoundSingleClip : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    // Define regions in seconds
    public float startTime = 0f;
    public float loopStart = 1f;
    public float loopEnd = 3f;
    public float endStart = 3f;

    private bool _isLooping = false;

    private void Update()
    {
        if (_isLooping && audioSource.isPlaying && audioSource.time >= loopEnd)
        {
            audioSource.time = loopStart;
        }
    }

    public void StartLoop()
    {
        if (_isLooping) return;
        _isLooping = true;

        audioSource.clip = audioClip;
        audioSource.loop = false;
        audioSource.time = startTime;
        audioSource.Play();
    }

    public void StopLoop()
    {
        if (!_isLooping) return;
        _isLooping = false;

        audioSource.time = endStart;
        audioSource.loop = false;
    }
}