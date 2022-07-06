using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class ThrowTarget : MonoBehaviour, IThrowProjectileTarget
{
    [SerializeField] private List<PointRange> pointRanges;

    private List<ThrowProjectile> projectilesOnTarget;
    public event Action<int> OnPointsScore;

    private void Awake()
    {
    }

    public void OnBladeHit(ThrowProjectile pProjectile,Vector3 hitPoint)
    {
        var delta = hitPoint - transform.position;
        var distance = delta.magnitude;
        int pointsScored = 1;

        foreach (var pointRange in pointRanges)
        {
            if (distance > pointRange.threshold)
            {
                pointsScored = pointRange.points;
                break;
            }
        }
        
        OnPointsScore?.Invoke(pointsScored);
    }

    public void OnHandleHit(ThrowProjectile pProjectile,Vector3 hitPoint)
    {
        
    }
}

[Serializable]
public struct PointRange
{
    public float threshold;
    public int points;
}
