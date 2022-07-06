using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class BottleFlipper : MonoBehaviour
{
    public UnityEvent OnSuccessfulFlip, OnFailedFlip;
    
    private XRGrabInteractable xrGrabInteractable;
    private Rigidbody rb;
    private bool isThrown = false;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        if (transform.position.y < -0.25f)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
        
        if (isThrown)
        {
            if (rb.IsSleeping())
            {
                var dot = Vector3.Dot(transform.up, Vector3.up);
                if (dot > 0.97f)
                    OnSuccessfulFlip.Invoke();
                else
                    OnFailedFlip.Invoke();
                
                isThrown = false;
            }
        }
    }

    public void OnThrow()
    {
        isThrown = true;
    }
}
