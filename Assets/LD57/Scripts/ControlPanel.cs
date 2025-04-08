using System;
using System.Collections;
using LD57.Scripts;
using UnityEngine;
using static Globals;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private TelescopeControls _telescopeControls;
    [SerializeField] private TelescopeSettings _telescopeSettings;
    [SerializeField] private InfoPanel _infoPanel;
    [SerializeField] private LokatorPanel _lokatorPanel;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private Slider3D _volume;

    [SerializeField] private ButtonLamp ZoomAndFocus;
    [SerializeField] private ButtonLamp TargetLock;
    [SerializeField] private ButtonLamp SendData;
    [SerializeField] private ButtonLamp Locator;
    [SerializeField] private ButtonLamp Start;
    [SerializeField] private ButtonLamp Monitor;

    [SerializeField] private GameObject StartGameScreen;
    [SerializeField] private GameObject EndGameScreen;
    private void Awake()
    {
        ZoomAndFocus.SetState(false);
        TargetLock.SetState(false);
        SendData.SetState(false);
        Locator.SetState(false);
        Start.SetState(true);
        Monitor.SetState(false);
        
        StartGameScreen.SetActive(true);
    }

    public void Init()
    {
        _telescopeControls.Init();
        _telescopeSettings.Init();
        _infoPanel.Init();
        _lokatorPanel.Init();
        _progressBar.Init();
        _volume.Init();

        G.Presenter.PlayerState.Subscribe(state =>
        {
            StartGameScreen.SetActive(state == GameStates.EnterGame);
            ZoomAndFocus.SetState(state == GameStates.ResearcObject);
            TargetLock.SetState(state == GameStates.Exploring && G.Presenter.DetectedObjectPower.Value > 50f);
            SendData.SetState(state == GameStates.ResearcObject && G.Presenter.ResearchProgress.Value > GamePreferences.MIN_COMPLET_RESERCH);
            Locator.SetState(state != GameStates.EnterGame && state != GameStates.EndGame);
            Start.SetState(state == GameStates.EnterGame);
            Monitor.SetState(state != GameStates.EnterGame && state != GameStates.EndGame);
        });
        
        G.Presenter.DetectedObjectPower.Subscribe(value =>
        {
            TargetLock.SetState( G.Presenter.PlayerState.Value == GameStates.Exploring && G.Presenter.DetectedObjectPower.Value > 50f);
        });
        
                
        G.Presenter.ResearchProgress.Subscribe(value =>
        {
            SendData.SetState( G.Presenter.PlayerState.Value == GameStates.ResearcObject && G.Presenter.ResearchProgress.Value > GamePreferences.MIN_COMPLET_RESERCH);
        });

        StartCoroutine(StartLamp());
    }
    
    private IEnumerator StartLamp()
    {
       
        float elapsedTime = 0f;
        var state = false;
        while (G.Presenter.PlayerState.Value == GameStates.EnterGame)
        {
            elapsedTime += Time.deltaTime;
      
            Start.SetState(state);
            state = !state;
            yield return new WaitForSeconds(0.5f);
        }

        Start.SetState(false);
    }
}