using System;
using System.Collections;
using System.Linq;
using LD57.Scripts;
using TMPro;
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

    [SerializeField] private SpriteRenderer StartGameScreen;
    [SerializeField] private GameObject EndGameScreen;

    [SerializeField] private Prize[] Prizes;
    [SerializeField] private TMP_Text TurnToggle;
    [SerializeField] private TMP_Text ScreenText;

    private readonly int _alphaProperty = Shader.PropertyToID("_Alpha");
    private readonly int _greyLuminosityProperty = Shader.PropertyToID("_GreyscaleLuminosity");
    private static readonly int alpha = Shader.PropertyToID("_Alpha");
    private const string GRAY_EFFECT = "GREYSCALE_ON";

    [Serializable]
    public class Prize
    {
        public SpriteRenderer Sticker;
        public Objects ObjectType;

        [HideInInspector] public Material SpriteMaterial;
    }
    
    public void Init()
    {
        
        ZoomAndFocus.SetState(false);
        TargetLock.SetState(false);
        SendData.SetState(false);
        Locator.SetState(false);
        Start.SetState(true);
        Monitor.SetState(false);
        StartGameScreen.gameObject.SetActive(true);

        foreach (var prize in Prizes)
        {
            prize.SpriteMaterial = prize.Sticker.material;
            prize.Sticker.material = prize.SpriteMaterial;

            prize.SpriteMaterial.EnableKeyword(GRAY_EFFECT);
            prize.SpriteMaterial.SetFloat(_greyLuminosityProperty, 1f);
            prize.SpriteMaterial.SetFloat(_alphaProperty, 0.15f);
        }
        
        _telescopeControls.Init();
        _telescopeSettings.Init();
        _infoPanel.Init();
        _lokatorPanel.Init();
        _progressBar.Init();
        _volume.Init();

        G.Presenter.PlayerState.Subscribe(state =>
        {
            ZoomAndFocus.SetState(state == GameStates.ResearcObject);
            TargetLock.SetState(state == GameStates.Exploring && G.Presenter.DetectedObjectPower.Value > 50f);
            SendData.SetState(state == GameStates.ResearcObject && G.Presenter.ResearchProgress.Value > GamePreferences.MIN_COMPLET_RESERCH);
            Locator.SetState(state != GameStates.EnterGame && state != GameStates.EndGame);
            Start.SetState(state == GameStates.EnterGame);
            Monitor.SetState(state != GameStates.EnterGame && state != GameStates.EndGame);

            if (G.Presenter.PlayerState.PreviousValue == GameStates.EnterGame)
            {
                StartCoroutine(StartGame());
            }
            
            if (G.Presenter.PlayerState.PreviousValue != GameStates.EndGame && state == GameStates.EndGame)
            {
                StartCoroutine(EndGame());
            }
        });

        G.Presenter.DetectedObjectPower.Subscribe(value => { TargetLock.SetState(G.Presenter.PlayerState.Value == GameStates.Exploring && G.Presenter.DetectedObjectPower.Value > 50f); });


        G.Presenter.ResearchProgress.Subscribe(value => { SendData.SetState(G.Presenter.PlayerState.Value == GameStates.ResearcObject && G.Presenter.ResearchProgress.Value > GamePreferences.MIN_COMPLET_RESERCH); });


        G.Presenter.ObjectWasReserched.Subscribe(spaceObject =>
        {
            var prize = Prizes.First(it => it.ObjectType == spaceObject.ObjectType);

            prize.SpriteMaterial.DisableKeyword(GRAY_EFFECT);
            prize.SpriteMaterial.SetFloat(_alphaProperty, 1f);
        });

        G.Presenter.LastObjectWasResearched.Subscribe(value =>
        {
            TurnToggle.text = "Turn OFF";
            StartCoroutine(BlinkLamp());
        });

        StartCoroutine(EnterGame());
        ScreenText.alpha = 0f;
    }

    private IEnumerator EnterGame()
    {
        Start.SetState(false,true);
        StartGameScreen.material.SetFloat(alpha, 1f);
        var duration = 2f;
        var elapsedTime = 0f;
        ScreenText.alpha = 0f;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            ScreenText.alpha =Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        _infoPanel.SetStartText();
        StartCoroutine(BlinkLamp());
        ScreenText.alpha = 1f;
    }

    private IEnumerator StartGame()
    {
        var duration = 1f;
        var elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            ScreenText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        duration = 2f;
        elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            StartGameScreen.material.SetFloat(alpha, Mathf.Lerp(1f, 0f, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartGameScreen.material.SetFloat(alpha, 0f);
    }

    private IEnumerator EndGame()
    {
        ScreenText.text = "Thank you for playing!";
        var duration = 2f;
        var elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            StartGameScreen.material.SetFloat(alpha, Mathf.Lerp(0f, 1f, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartGameScreen.material.SetFloat(alpha, 1f);
        
         duration = 1f;
         elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            ScreenText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }
        ScreenText.alpha = 1f;
    }

    private IEnumerator BlinkLamp()
    {
        var state = false;
        while (G.Presenter.PlayerState.Value == GameStates.EnterGame ||
               (G.Presenter.LastObjectWasResearched.Value &&
                G.Presenter.PlayerState.Value != GameStates.EndGame))
        {
            Start.SetState(state);
            state = !state;
            yield return new WaitForSeconds(0.5f);
        }

        
        Start.SetState(false);
    }
}