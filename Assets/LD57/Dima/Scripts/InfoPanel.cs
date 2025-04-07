using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Globals;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    public void Init()
    {
        //OnInfoPanelTextChange
        G.Presenter.OnInfoPanelTextChange.Subscribe(SetText);
    }

    private void SetText(string text)
    {
        _text.text = text;
    }
}
