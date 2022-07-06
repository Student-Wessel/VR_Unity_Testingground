using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRThrowInteractable : XRGrabInteractable
{
    private Rigidbody rb;
    
    [Header("Custom Throw properties")] 
    [SerializeField] private ThrowScaleValue[] throwScales;
    
    public event Action OnThrow;
    
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    protected override void Detach()
    {
        if (!throwOnDetach)
            return;
        
        if (throwOnDetach)
        {
            var throwMagnitude = GetDetachVelocity().magnitude;

            foreach (var throwScaleValue in throwScales)
            {
                if (throwScaleValue.threshold > throwMagnitude)
                {
                    rb.velocity = GetDetachVelocity() * throwScaleValue.throwScale;
                    rb.angularVelocity = GetDetachAngularVelocity() * throwScaleValue.throwAngularScale;
                    OnThrow?.Invoke();
                    break;
                }
            }
        }
        

    }
}

[Serializable]
public struct ThrowScaleValue
{
    public float threshold;
    public float throwScale;
    public float throwAngularScale;
}
