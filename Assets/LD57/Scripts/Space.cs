using UnityEngine;
using static Globals;

public class Space : MonoBehaviour
{
    
    public void Init()
    {
        G.Presenter.OnMove.Subscribe(direction =>
        {
            Debug.Log($"Move space {direction}");
        });
        
    }
}
