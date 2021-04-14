using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeWeapon : Weapon
{
    [Header("Fire")]
    [SerializeField] private GameObject fireBullet = null;

    [Header("Water")]
    [SerializeField] private GameObject waterBullet = null;
    [SerializeField] [Range(0f, 100f)] private float waterPower = 24f;
    [SerializeField] [Range(0f, 3f)] private float waterCharge = 1f;
    private float currentSpiral;
    private float stream;

    [Header("Tesla")]
    [SerializeField] private GameObject teslaBullet = null;
    [SerializeField] private GameObject chargeBall = null;

    protected override void ShootFire()
    {
        throw new System.NotImplementedException();
    }

    protected override void ShootTesla()
    {
        throw new System.NotImplementedException();
    }

    protected override void ShootWater()
    {
        currentSpiral = Mathf.Min(ammo.y, waterCharge * 4) / 4f;
        ammo.y -= Mathf.CeilToInt(currentSpiral);
        stream = 0f;
    }

    protected override void SubStart()
    {
        
    }

    protected override void SubUpdate()
    {
        if (currentSpiral > 0f)
        {
            currentSpiral -= Time.deltaTime;
            if (stream >= 1f / (waterPower/waterCharge)) SprayWater();
            else stream += Time.deltaTime;
        }
    }

    private void SprayWater()
    {
        Instantiate(waterBullet).GetComponent<Projectile>().ShootSpiral(bulletHole.position, cam.forward, waterPower, currentSpiral / (waterCharge * 4f));
        stream = 0f;
    }
}
