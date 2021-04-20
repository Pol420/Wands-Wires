using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand_Weapon : Weapon
{
    [Header("Physics")]
    [SerializeField] [Range(0f, 100f)] private float shotPower = 20f;
    [SerializeField] [Range(0f, 5f)] private float weight = 0f;
    [SerializeField] [Range(0f, 100f)] private float damage = 20f;
    [SerializeField] [Range(0f, 40f)] private float bps = 6f;

    protected override void Shoot(Ammo ammo)
    {
        GameObject bullet;
        if (ammo == Ammo.Fire) { bullet = Instantiate(fireBullet); SpendFire(); }
        else if (ammo == Ammo.Water) { bullet = Instantiate(waterBullet); SpendWater(); }
        else { bullet = Instantiate(teslaBullet); SpendTesla(); }
        bullet.GetComponent<Projectile>().ShootProjectile(bulletHole.position, cam.transform.forward, damage, shotPower, weight);
    }

    protected override void SubStart()
    {
        reloadTime = 1f / bps;
    }

    protected override void SubUpdate()
    {

    }
}
