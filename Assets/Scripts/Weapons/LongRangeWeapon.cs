using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeWeapon : Weapon
{

    [Header("Fire")]
    [SerializeField] [Range(0f, 100f)] private float firePower = 50f;
    [SerializeField] private GameObject fireBullet = null;
    [SerializeField] [Range(0f, 10f)] private float fireBulletWeight = 5f;
    [SerializeField] [Range(0f, 10f)] private float explosionRadius = 5f;

    [Header("Water")]
    [SerializeField] [Range(0f, 100f)] private float waterPower = 40f;
    [SerializeField] private GameObject waterBullet = null;
    [SerializeField] [Range(0f, 10f)] private float waterBulletWeight = 3f;
    [SerializeField] [Range(0f, 100f)] private float waterDensity = 30f;
    private bool shootingWater;
    private float remainingWater;
    private float densityCounter;

    [Header("Tesla")]
    [SerializeField] [Range(0f, 100f)] private float teslaPower = 60f;
    [SerializeField] private GameObject teslaBullet = null;
    [SerializeField] private GameObject chargeBall = null;
    [SerializeField] [Range(0f, 5f)] private float maxChargeTime = 5f;
    [SerializeField] [Range(0, 5)] private int teslaBounces = 3;
    private bool chargingTesla;
    private float chargeTime;
    private GameObject currentChargeBall;

    protected override void ShootFire()
    {
        FireBomb();
        AddFireAmmo(-1);
    }

    protected override void ShootTesla()
    {
        chargingTesla = true;
        chargeTime = 0f;
        currentChargeBall = Instantiate(chargeBall);
    }

    protected override void ShootWater()
    {
        shootingWater = true;
        remainingWater = GetWaterAmmo();
    }

    protected override void SubStart()
    {
        shootingWater = false;
        remainingWater = 0f;
        densityCounter = 0f;
    }

    protected override void SubUpdate()
    {
        if(shootingWater)
        {
            if (remainingWater <= 0f || !Input.GetButton("Fire1")) EndWaterStream();
            else
            {
                remainingWater -= Time.deltaTime;
                SetWaterAmmo(Mathf.FloorToInt(remainingWater));
                if (densityCounter >= 1f / waterDensity) WaterStream();
                else densityCounter += Time.deltaTime;
            }
        }
        else if (chargingTesla)
        {
            if (chargeTime >= maxChargeTime || !Input.GetButton("Fire1")) TeslaLaser();
            else
            {
                chargeTime += Time.deltaTime;
                currentChargeBall.transform.localScale = Vector3.one * (chargeTime + 1) / (maxChargeTime + 1);
                currentChargeBall.transform.position = bulletHole.position;
            }
        }
    }
    private void EndWaterStream() { shootingWater = false; reloadTime = ammoReloadTime.y; }

    private void FireBomb()
    {
        Instantiate(fireBullet).GetComponent<ThrownProjectile>().Shoot(bulletHole.position, cam.transform.forward + Vector3.up * 0.1f, firePower, fireBulletWeight, 2, 0, explosionRadius);
    }

    private void WaterStream()
    {
        Instantiate(waterBullet).GetComponent<ThrownProjectile>().Shoot(bulletHole.position, cam.transform.forward + Vector3.up * 0.05f, waterPower, waterBulletWeight, 1, 1);
        densityCounter = 0f;
    }

    private void TeslaLaser()
    {
        Destroy(currentChargeBall.gameObject);
        chargingTesla = false;
        Instantiate(teslaBullet).GetComponent<ThrownProjectile>().Shoot(bulletHole.position, cam.transform.forward, teslaPower * (chargeTime + 1) / (maxChargeTime + 1), 0, teslaBounces);
        reloadTime = ammoReloadTime.z;
        AddTeslaAmmo(-1);
    }
}
