using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// For stopping and starting the game (via setting timescale).
/// </summary>
public class SimplePause : MonoBehaviour
{
    
    [SerializeField] private bool shouldDisableAudioWhenPaused;
    
    public UnityEvent OnPaused;
    public UnityEvent OnResumed;
    
    public event Action OnPausedAction;
    public event Action OnResumedAction;
    
    private bool _isPaused;

    private float _originalTimescale;

    public void TogglePause()
    {
        if (_isPaused)
        {
           Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        _originalTimescale = Time.timeScale;
        Time.timeScale = 0;
        if (shouldDisableAudioWhenPaused)
        {
            AudioListener.pause = true;
        }
        _isPaused = true;
        OnPaused?.Invoke();
        OnPausedAction?.Invoke();
    }

    public void Resume()
    {
        Time.timeScale = _originalTimescale;
        if (shouldDisableAudioWhenPaused)
        {
            AudioListener.pause = false;
        }
        _isPaused = false;
        OnResumed?.Invoke();
        OnResumedAction?.Invoke();
    }
}
