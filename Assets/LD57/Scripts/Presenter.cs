using System;
using UnityEngine;
using Utils;


public class Presenter
{
    public SubscribableAction<Vector2> OnMove = new();
    public SubscribableAction<float> OnFocusChange = new();
    public SubscribableAction<float> OnZoom = new();
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
    
    //Controls
    public SubscribableAction<Vector2> OnControlButtonDown = new();
    public SubscribableAction OnControlButtonUp = new();
    public SubscribableAction<FiltersType> OnFilterButtonClick = new();
    
}