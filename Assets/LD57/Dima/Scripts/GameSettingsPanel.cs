using LD57.Scripts;
using UnityEngine;
using UnityEngine.UI;
using static Globals;

public class GameSettingsPanel : MonoBehaviour
{
    [SerializeField] private StartButton _startGameButton;
    [SerializeField] private Slider3D _volumeSlider;

    public void Init()
    {
        _startGameButton.Init();
    }

    
}
