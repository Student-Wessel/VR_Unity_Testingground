using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Mole : MonoBehaviour
{
    public UnityEvent Hit;
    
    public event Action<Mole,GameObject> leftHole;

    private Collider collider;
    private GameObject currentHole = null;
    private Transform startingParent;
    
    private Vector3 target;
    private float targetStayTime, digSpeed, height;
    private float timeInState;

    public bool moleHasBeenHit { get; private set; }

    private enum MoleState
    {
        STAY,
        UPDIG,
        DOWNDIG,
        NOTACTIVE
    }

    private MoleState currentState;
    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
        startingParent = transform.parent;
        target = transform.position;

        currentState = MoleState.NOTACTIVE;
    }
    
    private void Update()
    {
        timeInState += Time.deltaTime;
        switch (currentState)
        {
            case MoleState.STAY:
                OnStayUpdate();
                break;
            case MoleState.UPDIG:
                OnUpDigUpdate();
                break;
            case MoleState.DOWNDIG:
                OnDownDigUpdate();
                break;
            case MoleState.NOTACTIVE:
                break;
        }
    }

    // Stay Update
    private void OnStayUpdate()
    {
        if (targetStayTime < timeInState)
        {
            currentState = MoleState.DOWNDIG;
            timeInState = 0;
            target = currentHole.transform.position + Vector3.down * height;
        }
    }

    // Up Dig Update
    private void OnUpDigUpdate()
    {
        var step = digSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if ((transform.position - target).magnitude < 0.01f)
        {
            transform.position = target;
            currentState = MoleState.STAY;
            timeInState = 0;
        }
    }
    
    // Down Dig Update
    private void OnDownDigUpdate()
    {
        var step = digSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        
        if ((transform.position - target).magnitude < 0.01f)
        {
            transform.position = target;
            currentState = MoleState.NOTACTIVE;
            timeInState = 0;
            collider.enabled = false;
            transform.SetParent(startingParent);
            transform.localPosition = Vector3.zero;
            leftHole.Invoke(this,currentHole);
            currentHole = null;
        }
    }

    public void OnHit()
    {
        timeInState = 0;
        target = currentHole.transform.position + Vector3.down * height;
        currentState = MoleState.DOWNDIG;
        collider.enabled = false;
        moleHasBeenHit = true;
        Hit.Invoke();
    }

    public void Activate(GameObject pHole,float pStayTime,float pDigSpeed, float pHeight)
    {
        moleHasBeenHit = false;
        currentHole = pHole;
        targetStayTime = pStayTime;
        digSpeed = pDigSpeed;
        height = pHeight;
        timeInState = 0;
        collider.enabled = true;

        var moleTransform = transform;
        moleTransform.SetParent(currentHole.transform);
        moleTransform.localPosition = Vector3.zero;
        moleTransform.position += Vector3.down * height;

        target = currentHole.transform.position;

        currentState = MoleState.UPDIG;
    }
}
