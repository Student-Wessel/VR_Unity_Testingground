using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SimpleWorldButton : MonoBehaviour
{
    [SerializeField] private string targetTag = "PlayerHand";
    [SerializeField] private GameObject innerButton;

    private Vector3 targetPosition;
    
    private bool isPressed = false;
    
    public UnityEvent OnPress;
    
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Update()
    {
        if (isPressed)
        {
            innerButton.transform.position = Vector3.MoveTowards(innerButton.transform.position, targetPosition, Time.deltaTime * 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !isPressed)
        {
            OnPress.Invoke();
            isPressed = true;
            StartCoroutine(PressLoop());
        }
    }

    private IEnumerator PressLoop()
    {
        targetPosition = innerButton.transform.position + (Vector3.down * 0.01f);
        yield return new WaitForSeconds(0.5f);
        targetPosition = innerButton.transform.position + (Vector3.up * 0.01f);
        yield return new WaitForSeconds(0.5f);
        isPressed = false;
    }
}
