using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRThrowInteractable))]
public class ThrowProjectile : MonoBehaviour
{
    [SerializeField] private List<GameObject> handleGameObjects,bladeGameObjects;
    [SerializeField] private LayerMask stuckableLayers;
    
    public UnityEvent OnThrowUnity,OnStuck,OnThumble;


    private XRThrowInteractable throwInteractable;
    private Rigidbody rb;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public enum ProjectileState
    {
        START,
        THROWING,
        TUMBLING,
        STUCK
    }

    private ProjectileState currentState = ProjectileState.START;
    public ProjectileState CurrentState => currentState;
    
    private void Awake()
    {
        throwInteractable = GetComponent<XRThrowInteractable>();
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }
    private void Start()
    {
        throwInteractable.OnThrow += OnThrow;
    }

    private void OnCollisionEnter(Collision other)
    {
        var subColliderGameObject = other.contacts[0].thisCollider.gameObject;
        var throwProjectileTarget = other.gameObject.GetComponent<IThrowProjectileTarget>();
        
        if (bladeGameObjects.Contains(subColliderGameObject))
        {
            if (currentState == ProjectileState.THROWING && stuckableLayers == (stuckableLayers | (1 << other.gameObject.layer)))
            {
                currentState = ProjectileState.STUCK;
                rb.isKinematic = true;
                rb.useGravity = false;
                if (throwProjectileTarget != null)
                    throwProjectileTarget.OnBladeHit(this,other.contacts[0].point);
                transform.SetParent(other.transform);
                OnStuck.Invoke();
            }
        }
        else if (handleGameObjects.Contains(subColliderGameObject))
        {
            if (currentState == ProjectileState.THROWING)
            {
                currentState = ProjectileState.TUMBLING;
                OnThumble.Invoke();
            }
        }
    }

    private void OnThrow()
    {
        OnThrowUnity.Invoke();
        rb.isKinematic = false;
        currentState = ProjectileState.THROWING;
    }

    public void ResetToStart()
    {
        currentState = ProjectileState.START;
        rb.isKinematic = false;

        transform.rotation = startRotation;
        transform.position = startPosition;
    }
}
