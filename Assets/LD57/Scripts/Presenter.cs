using System;
using UnityEngine;
using Utils;


public class Presenter
{
    public SubscribableAction<Vector2> OnMove = new();
    public SubscribableField<float> OnFocusChange = new();
    public SubscribableField<float> OnZoom = new();
    public SubscribableAction<Enum, bool> OnFilterSet = new();
    public SubscribableAction<bool> OnEndOfReserchButtonClicked = new();
    public SubscribableAction<bool> OnReserchIsDone = new();
    public SubscribableAction<string> OnInfoPanelTextChange = new();
    public SubscribableAction<Vector2> OnTargetAcquisition = new();
    public SubscribableAction<Vector2> OnTargetAreaEnter = new();
    public SubscribableAction<int> OnGameStarted = new();
    public SubscribableAction<float> OnGameVolumeChange = new();
    public SubscribableAction<float> OnFrequencyChenge = new();
    public SubscribableAction<float> OnLocation = new();

    
    
    //Gameplay
    public SubscribableField<GameStates> PlayerState = new(GameStates.EnterGame);
    public SubscribableAction OnStartResearch = new();
    public SubscribableAction OnSendData = new();
    
    public SubscribableAction<string> SendText = new();
    public SubscribableField<float> DetectedObjectPower = new();
    public SubscribableField<int> TelescopePower = new();
    public SubscribableField<float> ResearchProgress = new();
    
    public SubscribableField<Vector2> TelescopeRotation = new();
    public SubscribableField<bool> TelescopeRotationXMax = new();
    public SubscribableField<bool> TelescopeRotationYMax = new();

    public SubscribableField<InSpaceObject> DetectedObject = new();
    
    public SubscribableAction<InSpaceObject> ObjectWasReserched = new();
    //Settings
    public SubscribableAction OnStartGame = new();
    public SubscribableAction OnEndGame = new();
    public SubscribableField<float> MusicVolume = new();
    
    //Controls
    public SubscribableAction<Vector2> OnControlButtonDown = new();
    public SubscribableAction OnControlButtonUp = new();
    public SubscribableAction<FiltersType> OnFilterButtonClick = new();
    
}