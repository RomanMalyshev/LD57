using UnityEngine;
using static Globals;

public class TargetButton : MonoBehaviour
{
    public AudioSource ButtSound;
    private void OnMouseDown()
    {
        if(ButtSound != null)ButtSound.Play();
        G.Presenter.OnStartResearch?.Invoke();
    }
}
