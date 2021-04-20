using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffWeapon : Weapon
{
    [Header("Physics")]
    [SerializeField] [Range(0f, 100f)] private float shotPower = 60f;
    [SerializeField] [Range(0f, 100f)] private float damage = 20f;
    [SerializeField] [Range(0f, 100f)] private float weight = 5f;
    [SerializeField] [Range(0, 10)] private int bullets = 5;
    [SerializeField] [Range(0f, 1f)] private float dispersion = 0.15f;

    protected override void Shoot(Ammo ammo)
    {
        int shots = Mathf.Min(bullets, GetCurrentAmmo());
        for (int i=0; i < shots; ++i)
        {
            GameObject bullet;
            if (ammo == Ammo.Fire) { bullet = Instantiate(fireBullet); SpendFire(); }
            else if (ammo == Ammo.Water) { bullet = Instantiate(waterBullet); SpendWater(); }
            else { bullet = Instantiate(teslaBullet); SpendTesla(); }
            Vector2 dispersionCircle = Random.insideUnitCircle * dispersion;
            bullet.GetComponent<Projectile>().ShootProjectile(bulletHole.position, cam.transform.forward + cam.transform.up * dispersionCircle.x + cam.transform.right * dispersionCircle.y, damage, shotPower, weight);
        }
    }

    protected override void SubStart()
    {

    }

    protected override void SubUpdate()
    {

    }
}
