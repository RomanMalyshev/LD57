using Unity.VisualScripting;
using UnityEngine;
using static Globals;

public class ControlPanel : MonoBehaviour
{
   [SerializeField] private TelescopeControls _telescopeControls;
   [SerializeField] private TelescopeSettings _telescopeSettings;
   [SerializeField] private InfoPanel _infoPanel;

   public void Init()
   {
      _telescopeControls.Init();
      _telescopeSettings.Init();
      _infoPanel.Init();
      //Example
      //G.Presenter.OnMove?.Invoke(Vector2.One);
   }
   
}
