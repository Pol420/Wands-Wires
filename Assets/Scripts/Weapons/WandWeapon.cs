using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandWeapon : Weapon
{
    [Header("Physics")]
    [SerializeField] [Range(0f, 40f)] private float bps = 6f;

    protected override void Shoot(GameObject bullet)
    {
        bullet.GetComponent<Projectile>().ShootProjectile(bulletHole.position, cam.transform.forward, Damage(), ShotPower(), weight);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Player/rafaga", GetComponent<Transform>().position);
    }

    protected override void SubStart()
    {
        reloadTime = 1f / bps;
    }

    protected override void SubUpdate()
    {

    }
}
