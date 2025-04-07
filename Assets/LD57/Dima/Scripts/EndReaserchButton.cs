using UnityEngine;
using static Globals;

public class EndReaserchButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        G.Presenter.OnSendData?.Invoke();
    }
}
