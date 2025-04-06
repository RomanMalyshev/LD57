using UnityEngine;
using UnityEngine.UI;
using static Globals;

public class GameSettingsPanel : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Slider _volumeSlider;

    public void Init()
    {
        _volumeSlider.onValueChanged.AddListener(VolumeChange);
        _startGameButton.onClick.AddListener(GameStart);
    }

    private void VolumeChange(float volumeValue)
    {
        _volumeSlider.value = volumeValue;
        G.Presenter.OnGameVolumeChange?.Invoke(_volumeSlider.value);
        Debug.Log("Game Volume Change"+volumeValue);
    }

    private void GameStart()
    {
        G.Presenter.OnGameStarted?.Invoke(1);
        Debug.Log("Game Started");
    }
}
