using UnityEngine;

public class ControlPanel : MonoBehaviour
{
   [SerializeField] private TelescopeControls _telescopeControls;
   [SerializeField] private TelescopeSettings _telescopeSettings;
   [SerializeField] private InfoPanel _infoPanel;
   [SerializeField] private LokatorPanel _lokatorPanel;
   [SerializeField] private ProgressBar _progressBar;

   public void Init()
   {
      _telescopeControls.Init();
      _telescopeSettings.Init();
      _infoPanel.Init();
      _lokatorPanel.Init();
      _progressBar.Init();
   }
   
}
