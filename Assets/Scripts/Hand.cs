using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    [SerializeField] private float animationSpeed;
    [SerializeField] private string animatorGripParam = "Grip",animatorTriggerParam = "Trigger";

    private Animator animator;
    private float gripTarget, triggerTarget, gripCurrent, triggerCurrent;
    
    private readonly int Grip = Animator.StringToHash("Grip");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimateHand();
    }

    public void SetGrip(float value)
    {
        gripTarget = value;
    }

    public void SetTrigger(float value)
    {
        triggerTarget = value;
    }

    void AnimateHand()
    {
        if (Math.Abs(gripCurrent - gripTarget) > 0.01f)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorGripParam,gripCurrent);
        }
        
        if (Math.Abs(triggerCurrent - triggerTarget) > 0.01f)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorTriggerParam,triggerCurrent);
        }
    }
}
