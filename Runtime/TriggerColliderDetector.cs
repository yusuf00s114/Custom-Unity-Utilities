using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerColliderDetector : MonoBehaviour
{
    public UnityEvent<Collider> OnTriggerEntered;
    public UnityEvent<Collider> OnTriggerExited;

    public event Action<Collider> OnTriggerEnteredAction;
    public event Action<Collider> OnTriggerExitedAction;
    
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEntered?.Invoke(other);
        OnTriggerEnteredAction?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExited?.Invoke(other);
        OnTriggerExitedAction?.Invoke(other);
    }
}
