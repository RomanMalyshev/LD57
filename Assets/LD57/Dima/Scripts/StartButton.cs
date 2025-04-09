using System;
using UnityEngine;
using static Globals;

public class StartButton : MonoBehaviour
{
    public ButtonLamp Lamp;
    public AudioSource ButtSound;

    private void OnMouseDown()
    {
        if (ButtSound != null) ButtSound.Play();
        G.Presenter.OnStartGame?.Invoke();
    }
}