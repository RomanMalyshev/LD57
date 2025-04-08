using System.Collections;
using LD57.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Globals;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _textDelay = 0.5f;
    private float _researchProgress;
    private InSpaceObject _object;
    private string _objectText;
    private bool _isTextisDone;
    private GameStates _gamestate;
    public void Init()
    {
        //OnInfoPanelTextChange
        //G.Presenter.OnInfoPanelTextChange.Subscribe(SetText);
        G.Presenter.DetectedObject.Subscribe(GetText);
        G.Presenter.ResearchProgress.Subscribe(SetText);
        G.Presenter.OnSendData.Subscribe(SetSearchingText);
        G.Presenter.PlayerState.Subscribe(SetState);
        _text.text = "";
        StartCoroutine(c_Output(GamePreferences.HeloText));
    }
    
    private void SetState(GameStates state)
    {
        _gamestate = state;
    }

    private void GetText(InSpaceObject obj)
    {
        if (obj != null)
        {
            switch (obj.ObjectType)
            {
                case Objects.Earth:
                    _objectText = GamePreferences.ZerathPrime;
                    break;
                case Objects.Galaxy:
                    _objectText = GamePreferences.SBcClassGalaxy;
                    break;
                case Objects.Inferno:
                    _objectText = GamePreferences.Infernoshard;
                    break;
                case Objects.Prime:
                    _objectText = GamePreferences.NavorisPrime;
                    break;
                case Objects.CosmoExpress:
                    _objectText = GamePreferences.StarcruiserNebulaSerpent;
                    break;
                case Objects.Sun:
                    _objectText = GamePreferences.BlazingStar;
                    break;
                case Objects.CosmoStation:
                    _objectText = GamePreferences.SpaceStationARGOS9;
                    break;
                case Objects.SpaceStation:
                    _objectText = GamePreferences.OrbitalStationHorizon;
                    break;
            }
        }
    }

    private void SetText(float progress)
    {
        if (progress >= GamePreferences.MIN_COMPLET_RESERCH && !_isTextisDone && _gamestate == GameStates.ResearcObject)
        {
            _isTextisDone = true;
            Debug.Log("Set Text");
            _text.text = "";
            StartCoroutine(c_Output(_objectText));
            //progress = 0;
        }
    }

    private void SetSearchingText()
    {
        StopAllCoroutines();
        _isTextisDone = false;
        _text.text = "";
        StartCoroutine(c_Output("Searching..."));
    }
    
    IEnumerator c_Output(string text)
    {
        foreach (var chr in text)
        {
            _text.text += chr;
            yield return new WaitForSeconds(_textDelay);
        }
    }
}
