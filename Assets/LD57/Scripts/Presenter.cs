using System;
using UnityEngine;
using Utils;
//using Vector2 = System.Numerics.Vector2;


public class Presenter
{
    public SubscribableAction<Vector2> OnMove = new();
    public SubscribableAction<float> OnFocusChange = new();
    public SubscribableAction<float> OnZoom = new();
    public SubscribableAction<Enum, bool> OnFilterSet = new();
    public SubscribableAction<bool> OnEndOfReserchButtonClicked = new();
    public SubscribableAction<bool> OnReserchIsDone = new();
    public SubscribableAction<string> OnInfoPanelTextChange = new();
    
    
    
}