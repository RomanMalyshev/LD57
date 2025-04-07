using System;
using LD57.Scripts;
using SingularityGroup.HotReload;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals G = null;
    public Presenter Presenter { get; private set; }

    [SerializeField] private Space _space;
    [SerializeField] private ControlPanel _controlPanel;
    [SerializeField] private Logic _logic;
    [SerializeField] private ControlPanelTest _controlPanelTest;
    
    //Game enter point
    public void Start()
    {
        G = this;
        Presenter = new Presenter();
        _logic.Init();
        _space.Init();
        _controlPanel.Init();
        _controlPanelTest.Init();
    }

}
