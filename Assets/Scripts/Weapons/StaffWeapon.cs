using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffWeapon : Weapon
{
    [Header("Physics")]
    [SerializeField] [Range(0, 10)] private int bullets = 5;
    [SerializeField] [Range(0f, 1f)] private float dispersion = 0.15f;

    protected override void Shoot(GameObject bullet)
    {
        int shots = Mathf.Min(bullets, GetCurrentAmmo());
        for (int i = 0; i < shots; ++i)
        {
            if (i != 0) SpendAmmo();
            Vector2 dispersionCircle = Random.insideUnitCircle * dispersion;
            Instantiate(bullet).GetComponent<Projectile>().ShootProjectile(bulletHole.position, cam.transform.forward + cam.transform.up * dispersionCircle.x + cam.transform.right * dispersionCircle.y, damage * holder.GetDamageMultiplier(), shotPower, weight);
        }
        Destroy(bullet);
    }

    protected override void SubStart()
    {

    }

    protected override void SubUpdate()
    {

    }
}
