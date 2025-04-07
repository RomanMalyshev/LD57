using System.Collections.Generic;
using UnityEngine;
using static Globals;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private List<ButtonLamp> _buttonLamps;
    public void Init()
    {
        G.Presenter.ResearchProgress.Subscribe(SetProgress);
        
        for (int i = 0; i < _buttonLamps.Count; i++)
        {
            _buttonLamps[i].on = false;
        }
    }

    private void SetProgress(float progress)
    {
       for (int i = 0; i < (int)progress/10; i++)
       {
           _buttonLamps[i].on = true;
       }
       
       for (int i = (int)progress/10; i < _buttonLamps.Count; i++)
       {
           _buttonLamps[i].on = false;
       }
    }
}
