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
        if (progress < 0.2f)
        {
            progress = 0;
        }

        for (int i = 0; i < Mathf.CeilToInt(progress / 10); i++)
        {
            if(_buttonLamps[i].on)continue; 
            _buttonLamps[i].SetState(true);
        }

        for (int i = Mathf.CeilToInt(progress / 10); i < _buttonLamps.Count; i++)
        {
             if(_buttonLamps[i].on  == false)continue; 
            _buttonLamps[i].SetState(false);
        }
    }
}