using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionObservable : MonoBehaviour
{
    [SerializeField] private bool ListenForEnter = true, ListenForExit = true, ListenForStay = false;
    public event Action<Collision, CollisionEventType> OnCollisionEvent;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision enter");
        if (!ListenForEnter)
            return;
        
        OnCollisionEvent?.Invoke(other,CollisionEventType.ENTER);
    }

    private void OnCollisionExit(Collision other)
    {
        if (!ListenForExit)
            return;
        
        OnCollisionEvent?.Invoke(other,CollisionEventType.EXIT);
    }

    private void OnCollisionStay(Collision other)
    {
        Debug.Log("Collision stay");
        if (!ListenForStay)
            return;
        
        OnCollisionEvent?.Invoke(other,CollisionEventType.STAY);
    }
}

public enum CollisionEventType{
    ENTER,
    EXIT,
    STAY
}
