using System;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Tooltip("For display purposes only; doesn't do anything.")]
    public string timerName;
    
    [Tooltip("In seconds.")] 
    public float duration;

    [Tooltip("If false, the timer will run again after stopping.")]
    public bool isOneShot;
    
    public bool ignoreTimescale;
    
    public float speed = 1f;

    [Header("Debug")] 
    public bool enableDebug;
    public bool logTimeLeft;

    public UnityEvent OnTimerStarted = new();
    public UnityEvent OnTimerStopped = new();
    public UnityEvent OnTimerAborted = new();
    public UnityEvent OnTimerPaused = new();
    public UnityEvent OnTimerUnpaused = new();
    
    public event Action OnTimerStartedAction;
    public event Action OnTimerStoppedAction;
    public event Action OnTimerAbortedAction;
    public event Action OnTimerPausedAction;
    public event Action OnTimerUnpausedAction;

    public bool IsRunning { get; private set; }
    public bool IsPaused { get; private set; }
    public float TimeLeft { get; private set; }

    private void Update()
    {
        if (!ignoreTimescale) return;
        HandleTimer(Time.unscaledDeltaTime);
    }

    private void FixedUpdate()
    {
        if (ignoreTimescale) return;
        HandleTimer(Time.deltaTime);
    }

    // If duration is -1, uses the value from the duration field
    public void Begin(float duration = -1f)
    {
        if (duration < 0)
        {
            TimeLeft = this.duration;
        }
        else
        {
            TimeLeft = duration;
        }
        if (enableDebug)
        {
            Debug.Log("[TIMER] Timer Started with " + TimeLeft + " seconds");
            Debug.Log("[TIMER] Timer duration is: " + this.duration);
        }

        IsRunning = true;
        OnTimerStarted?.Invoke();
        OnTimerStartedAction?.Invoke();
    }

    public void Pause()
    {
        IsPaused = true;
        OnTimerPaused?.Invoke();
        OnTimerPausedAction?.Invoke();
    }

    public void Unpause()
    {
        IsPaused = false;
        OnTimerUnpaused?.Invoke();
        OnTimerUnpausedAction?.Invoke();
    }

    public void Stop()
    {
        if (enableDebug) Debug.Log("[TIMER] Timer stopped");
        IsRunning = false;
        TimeLeft = 0f;
        OnTimerStopped?.Invoke();
        OnTimerStoppedAction?.Invoke();
        if (!isOneShot) Begin();
    }

    /// <summary>
    /// Stops the timer, but if respectOneShot is true, the timer will restart if it is a one-shot timer.
    /// </summary>
    public void Abort(bool respectOneShot = false)
    {
        if (enableDebug) Debug.Log("[TIMER] Timer stopped");
        IsRunning = false;
        TimeLeft = 0f;
        OnTimerAborted?.Invoke();
        OnTimerAbortedAction?.Invoke();
        if (respectOneShot && !isOneShot) Begin();
    }

    private void HandleTimer(float delta)
    {
        if (IsRunning && !IsPaused)
        {
            if (logTimeLeft)
            {
                Debug.Log("[TIMER] Time left: " + TimeLeft);
            }
            TimeLeft -= delta * speed;
            if (TimeLeft <= 0f) Stop();
        }
    }
}