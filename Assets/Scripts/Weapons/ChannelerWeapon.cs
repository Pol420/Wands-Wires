using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelerWeapon : Weapon
{
    [Header("Physics")]
    [SerializeField] [Range(0f, 5f)] private float maximumChargeTime = 3f;
    private float charge;
    private bool charging;
    private GameObject bullet;
    private Vector3 startingScale;

    protected override void Shoot(GameObject bullet)
    {
        AddAmmo();
        if (reloadTime == 0f)
        {
            charging = true;
            if (this.bullet == null)
            {
                this.bullet = bullet;
                startingScale = bullet.transform.localScale;
            }
            else Destroy(bullet);
            this.bullet.transform.position = bulletHole.transform.position;
            this.bullet.transform.localScale = startingScale * (0.5f + charge / maximumChargeTime);
            this.bullet.transform.up = cam.transform.forward;
            if (charge >= maximumChargeTime) ShootCharge();
            else charge += Time.deltaTime;
        }
        else Destroy(bullet);
    }

    private void ShootCharge()
    {
        charge = Mathf.Min(maximumChargeTime, charge);
        float proportion = 2f * charge / maximumChargeTime;
        SpendAmmo(Mathf.RoundToInt(charge));
        bullet.GetComponent<Projectile>().ShootProjectile(bulletHole.position, cam.transform.forward, damage * proportion * holder.GetDamageMultiplier(), shotPower);
        charge = 0f;
        charging = false;
        bullet = null;
        reloadTime = 1f;
        Invoke("ResetCharge", 1f);
    }

    private void ResetCharge() { reloadTime = 0f; }

    protected override void SubStart()
    {
        ResetCharge();
        charge = 0f;
    }

    protected override void SubUpdate()
    {
        if (charging) charging = false;
        else if (charge > 0f) ShootCharge();
    }
}
