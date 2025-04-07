using Unity.VisualScripting;
using UnityEngine;
using static Globals;

public class ControlPanel : MonoBehaviour
{
   [SerializeField] private TelescopeControls _telescopeControls;
   [SerializeField] private TelescopeSettings _telescopeSettings;
   [SerializeField] private InfoPanel _infoPanel;
   [SerializeField] private GameSettingsPanel _gameSettingsPanel;
   [SerializeField] private LokatorPanel _lokatorPanel;
   [SerializeField] private ProgressBar _progressBar;

   public void Init()
   {
      _telescopeControls.Init();
      _telescopeSettings.Init();
      _infoPanel.Init();
      _gameSettingsPanel.Init();
      _lokatorPanel.Init();
      _progressBar.Init();
      //Example
      //G.Presenter.OnMove?.Invoke(Vector2.One);
   }
   
}
