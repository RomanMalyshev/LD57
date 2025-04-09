using UnityEngine;
using static Globals;

public class EndReaserchButton : MonoBehaviour
{
    public AudioSource ButtSound;
    private void OnMouseDown()
    {
        
        if(ButtSound != null)ButtSound.Play();
        G.Presenter.OnSendData?.Invoke();
    }
}
