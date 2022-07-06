using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ResetToPositionAfterSec : MonoBehaviour
{
    [SerializeField] private float timeToReset = 5f;

    private Vector3 beginPosition;
    private Quaternion beginRotation;
    
    void Start()
    {
        beginPosition = transform.position;
        beginRotation = transform.rotation;
        StartCoroutine(ResetAfterTime());
    }


    private IEnumerator ResetAfterTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToReset);
            transform.position = beginPosition;
            transform.rotation = beginRotation;
        }
    }
}
