using LD57.Scripts;
using UnityEngine;
using static Globals;

public class Logic : MonoBehaviour
{
    public void Init()
    {
        G.Presenter.PlayerState.Value = GameStates.EnterGame;


        G.Presenter.OnStartGame.Subscribe(() =>
        {
            if (G.Presenter.PlayerState.Value == GameStates.EnterGame)
                G.Presenter.PlayerState.Value = GameStates.Exploring;
            else
                Debug.LogWarning("Trying to start, from wrong state");
        });

        G.Presenter.OnStartResearch.Subscribe(() =>
        {
            if (G.Presenter.PlayerState.Value == GameStates.Exploring &&
                G.Presenter.DetectedObject.Value != null)
                G.Presenter.PlayerState.Value = GameStates.Researching;
            else
                Debug.LogWarning("Trying to start, from wrong state");
        });

        G.Presenter.OnSendData.Subscribe(() =>
        {
            if (G.Presenter.PlayerState.Value == GameStates.Researching)
                G.Presenter.PlayerState.Value = GameStates.Exploring;
            else
                Debug.LogWarning("Trying to start, from wrong state");
        });

        G.Presenter.OnEndGame.Subscribe(() =>
        {
            if (G.Presenter.PlayerState.Value != GameStates.EnterGame)
                G.Presenter.PlayerState.Value = GameStates.EndGame;
            else
                Debug.LogWarning("Trying to start, from wrong state");
        });

        G.Presenter.TelescopeRotation.Subscribe(rotation =>
        {
            if (rotation.x >= GamePreferences.MAX_PITCH - 1f &&
                !G.Presenter.TelescopeRotationXMax.Value)
            {
                G.Presenter.TelescopeRotationXMax.Value = true;
            }
            
            if (rotation.x <= GamePreferences.MIN_PITCH + 1f &&
                !G.Presenter.TelescopeRotationXMax.Value)
            {
                G.Presenter.TelescopeRotationXMax.Value = true;
            }

            if (rotation.x > GamePreferences.MIN_PITCH + 1f &&
                rotation.x < GamePreferences.MAX_PITCH - 1f &&
                G.Presenter.TelescopeRotationXMax.Value)
            {
                G.Presenter.TelescopeRotationXMax.Value = false;
            }

            if (rotation.y >= GamePreferences.MAX_YOW - 1f &&
                !G.Presenter.TelescopeRotationXMax.Value)
            {
                G.Presenter.TelescopeRotationYMax.Value = true;
            }

            if (rotation.y <= GamePreferences.MIN_YOW + 1f &&
                !G.Presenter.TelescopeRotationYMax.Value)
            {
                G.Presenter.TelescopeRotationYMax.Value = true;
            }
            
            if (rotation.y > GamePreferences.MIN_YOW + 1f &&
                rotation.y < GamePreferences.MAX_YOW - 1f &&
                G.Presenter.TelescopeRotationYMax.Value)
            {
                G.Presenter.TelescopeRotationYMax.Value = false;
            }
        });
    }
}

public enum GameStates
{
    EnterGame,
    Exploring,
    Researching,
    Prize,
    EndGame
}