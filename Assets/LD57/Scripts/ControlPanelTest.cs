using System;
using UnityEngine;
using static Globals;

public class ControlPanelTest : MonoBehaviour
{
    [Range(0f, 1f)] public float Zoom;
    [Range(0f, 1f)] public float Focus;
    [Space] public bool FilterOptic;
    public bool FilterRadio;
    public bool FilterInfrared;
    public bool FilterUV;

    [Space]
    [Header("Show, do not touch")]
    [Range(0f, 1f)]
    public float DetectionObjectPower;

    [Range(0, 100)] public int TelescopePower;
    [Range(0f, 1f)] public float ResearchProgress;
    public int TelescopeXRotation;
    public int TelescopeYRotation;
    public bool MaxXRotation;
    public bool MaxYRotation;
    [TextArea] public string OnPanelText;

    [Space]
    [Header("Game Settings")]
    [Range(0f, 1f)]
    public float MusicVolume;

    [InspectorButton]
    public void StartResearch()
    {
        Debug.Log("StartResearch");
        G.Presenter.OnStartResearch?.Invoke();
    }

    [InspectorButton]
    public void SendData()
    {
        Debug.Log("SendData");
        G.Presenter.OnSendData?.Invoke();
    }

    [InspectorButton]
    public void StartGame()
    {
        Debug.Log("Start");
        G.Presenter.OnStartGame?.Invoke();
    }

    [InspectorButton]
    public void EndGame()
    {
        Debug.Log("EndGame");
        G.Presenter.OnEndGame?.Invoke();
    }


    //Radio telescope
    public void Init()
    {
        //In
        //Todo Test Animation
        G.Presenter.SendText.Subscribe(text => { OnPanelText = text; });
        G.Presenter.DetectedObjectPower.Subscribe(power => { DetectionObjectPower = power; });
        G.Presenter.TelescopePower.Subscribe(power => { TelescopePower = power; });
        G.Presenter.ResearchProgress.Subscribe(progress => { ResearchProgress = progress; });
        G.Presenter.TelescopeRotation.Subscribe(rotation =>
        {
            TelescopeXRotation = (int)Math.Ceiling(rotation.x);
            TelescopeYRotation = (int)Math.Ceiling(rotation.y);
        });
        
        G.Presenter.TelescopeRotationXMax.Subscribe(state => { MaxXRotation = state; });
        G.Presenter.TelescopeRotationYMax.Subscribe(state => { MaxYRotation = state; });


        //Out
    }


    public void Update()
    {
        
        if(G.Presenter.PlayerState.Value != GameStates.Exploring)return;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
           var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
           G.Presenter.OnMove?.Invoke(direction);
        }
        
    }
}