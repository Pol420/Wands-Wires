using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeWeapon : Weapon
{
    [Header("Fire")]
    [SerializeField] private GameObject fireBullet = null;
    [SerializeField] [Range(0f, 100f)] private float firePower = 20f;
    [SerializeField] [Range(0f, 3f)] private float fireDuration = 1.5f;
    [SerializeField] [Range(0f, 50f)] private float fireDensity = 20f;
    private bool flaming;
    private float fuel;
    private float densityCounter;

    [Header("Water")]
    [SerializeField] private GameObject waterBullet = null;
    [SerializeField] [Range(0f, 100f)] private float waterPower = 24f;
    [SerializeField] [Range(0f, 3f)] private float waterCharge = 1f;
    private float currentSpiral;
    private float stream;

    [Header("Tesla")]
    [SerializeField] private GameObject teslaCharge = null;
    [SerializeField] private GameObject teslaSpark = null;
    [SerializeField] [Range(0f, 1f)] private float sparkChance = 0.25f;
    [SerializeField] [Range(0f, 30f)] private float chargeDuration = 10f;

    protected override void ShootFire()
    {
        flaming = true;
        fuel = GetFireAmmo();
    }

    protected override void ShootTesla()
    {
        LaunchCharge();
        AddTeslaAmmo(-1);
    }

    protected override void ShootWater()
    {
        currentSpiral = Mathf.Min(GetWaterAmmo(), waterCharge * 4) / 4f;
        AddWaterAmmo(-Mathf.CeilToInt(currentSpiral));
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
            holder.Push(-transform.forward, waterPower);
        }
        else if (flaming)
        {
            if (fuel <= 0f || !Input.GetButton("Fire1")) EndFiring();
            else
            {
                fuel -= Time.deltaTime;
                SetFireAmmo(Mathf.FloorToInt(fuel));
                if (densityCounter >= 1f / fireDensity) ThrowFire();
                else densityCounter += Time.deltaTime;
            }
        }
    }
    private void EndFiring() { flaming = false; reloadTime = ammoReloadTime.x; }

    private void ThrowFire()
    {
        Instantiate(fireBullet).GetComponent<SpecialProjectile>().ShootWobbly(bulletHole.position, cam.transform.forward, firePower, fireDuration);
        densityCounter = 0f;
    }

    private void SprayWater()
    {
        Instantiate(waterBullet).GetComponent<SpecialProjectile>().ShootSpiral(bulletHole.position, cam.forward, waterPower, currentSpiral / (waterCharge * 4f));
        stream = 0f;
    }

    private void LaunchCharge()
    {
        Instantiate(teslaCharge).GetComponent<TeslaCharge>().Shoot(bulletHole.position, cam.transform.forward + Vector3.up * 0.2f, sparkChance, chargeDuration, teslaSpark);
    }
}
