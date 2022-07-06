using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Class used to make it so you can grap an item anywhere on its y axis
[RequireComponent(typeof(XRGrabInteractable))]
public class VerticalGrabber : MonoBehaviour
{
    private XRGrabInteractable xrGrabInteractable;
    private Vector3 originalLocalPosition;
    
    private void Awake()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        originalLocalPosition = xrGrabInteractable.attachTransform.localPosition;
    }

    public void OnGrabAttach(SelectEnterEventArgs args)
    {
        var interactableTransform = xrGrabInteractable.attachTransform;
        var handTransform = args.interactorObject.GetAttachTransform(null);
        
        var interactableLocalPosition = interactableTransform.localPosition;
        var handLocalPosition = interactableTransform.InverseTransformPoint(handTransform.position);
        var difference = handLocalPosition - interactableLocalPosition;

        interactableTransform.localPosition = new Vector3(interactableLocalPosition.x, difference.y, interactableLocalPosition.z);
    }

    public void OnGrabDrop(SelectExitEventArgs args)
    {
        xrGrabInteractable.attachTransform.localPosition = originalLocalPosition;
    }
}
