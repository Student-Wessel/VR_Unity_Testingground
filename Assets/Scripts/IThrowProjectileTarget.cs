using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowProjectileTarget
{
    public void OnBladeHit(ThrowProjectile projectile,Vector3 hitPoint);
    public void OnHandleHit(ThrowProjectile projectile,Vector3 hitPoint);
}
