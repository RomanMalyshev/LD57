using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals G = null;
    public Presenter Presenter { get; private set; }

    [SerializeField] private Space _space;
    [SerializeField] private ControlPanel _controlPanel;

    //Game enter point
    public void Start()
    {
        G = this;
        Presenter = new Presenter();
        
        _space.Init();
        _controlPanel.Init();

    }
}
