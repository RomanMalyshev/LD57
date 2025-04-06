using System.Collections;
using UnityEngine;

public class ZoomSoundSingleClip : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip zoomClip;

    // Define regions in seconds
    public float startTime = 0f;
    public float loopStart = 1f;
    public float loopEnd = 3f;
    public float endStart = 3f;

    private bool isZooming = false;

    private void Update()
    {
        if (isZooming && audioSource.time >= loopEnd)
        {
            // Loop back to start of loop section
            audioSource.time = loopStart;
        }
    }

    public void StartZoom()
    {
        if (isZooming) return;
        isZooming = true;

        audioSource.clip = zoomClip;
        audioSource.time = startTime;
        audioSource.Play();

        // Start checking for loop in Update
        StartCoroutine(WaitForStartToFinish());
    }

    private IEnumerator WaitForStartToFinish()
    {
        yield return new WaitForSeconds(loopStart); // Wait until loop section
        if (isZooming)
        {
            audioSource.time = loopStart;
        }
    }

    public void StopZoom()
    {
        if (!isZooming) return;
        isZooming = false;

        // Jump to end section
        audioSource.time = endStart;
        audioSource.loop = false;
    }
}