using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightShot : Projectile
{
    protected override void Explode()
    {
        DestroyProjectile();
    }

    protected override void Shoot(Vector3 position, Vector3 direction, float power)
    {
        transform.position = position;
        transform.forward = direction;
        body.velocity = direction * power;
    }
}
