using System.Collections;
using LD57.Scripts;
using UnityEngine;
using static Globals;

public class Logic : MonoBehaviour
{
    public float focusReturnDuration = 0.7f; // Duration to return focus and zoom to zero

    private IEnumerator ReturnFocus()
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        float initialZoom = G.Presenter.OnZoom.Value;
        float initialFocus = G.Presenter.OnFocusChange.Value;

        while (elapsedTime < focusReturnDuration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / focusReturnDuration); // Interpolation factor

            G.Presenter.OnZoom.Value = Mathf.Lerp(initialZoom, 0f, t);
            G.Presenter.OnFocusChange.Value = Mathf.Lerp(initialFocus, 0f, t);
            G.Presenter.OnFocusSet?.Invoke(G.Presenter.OnFocusChange.Value);
            G.Presenter.OnZoomSet?.Invoke(G.Presenter.OnZoom.Value);
            yield return null; // Wait for the next frame
        }

        // Ensure values are exactly zero at the end
        G.Presenter.OnZoom.Value = 0f;
        G.Presenter.OnFocusChange.Value = 0f;
    }

    public void Init()
    {
        G.Presenter.PlayerState.Value = GameStates.EnterGame;

        G.Presenter.OnStartGame.Subscribe(() =>
        {
            if (G.Presenter.LastObjectWasResearched.Value)
            {
                G.Presenter.PlayerState.Value = GameStates.EndGame;
                return;
            }

            if (G.Presenter.PlayerState.Value == GameStates.EnterGame)
            {
                StartCoroutine(ReturnFocus());
                G.Presenter.PlayerState.Value = GameStates.Exploring;
            }
            else
                Debug.LogWarning("Trying to start, from wrong state");
        });

        G.Presenter.OnStartResearch.Subscribe(() =>
        {
            if (G.Presenter.PlayerState.Value == GameStates.Exploring &&
                G.Presenter.DetectedObject.Value != null && 
                !G.Presenter.DetectedObject.Value.Reserched)
                G.Presenter.PlayerState.Value = GameStates.ResearcObject;
            else
                Debug.LogWarning("Trying to start, from wrong state");
        });

        G.Presenter.OnSendData.Subscribe(() =>
        {
            if (G.Presenter.PlayerState.Value == GameStates.ResearcObject)
            {
                if (G.Presenter.ResearchProgress.Value > GamePreferences.MIN_COMPLET_RESERCH)
                {
                    G.Presenter.DetectedObject.Value.SetResearchedState(true);
                    G.Presenter.ObjectWasReserched?.Invoke(G.Presenter.DetectedObject.Value);
                    G.Presenter.PlayerState.Value = GameStates.Exploring;
                    G.Presenter.DetectedObjectPower.Value = 0;
                    StartCoroutine(ReturnFocus());
                }
                else
                {
                    Debug.LogWarning("Data can't be sent not enough progress");
                }
            }
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
        
        G.Presenter.OnGameVolumeChange.Subscribe(value =>
        {
            AudioListener.volume = value;
            Debug.Log(value);
        });
        
        AudioListener.volume =1f;
    }
}

public enum GameStates
{
    EnterGame,
    Exploring,
    ResearcObject,
    Prize,
    EndGame
}