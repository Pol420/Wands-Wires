using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaPrism : Weapon
{

    [Header("Rays")]
    [SerializeField] private GameObject lightRayPrefab = null;
    [SerializeField] private Material fireMat = null;
    [SerializeField] private Material waterMat = null;
    [SerializeField] private Material teslaMat = null;

    [Header("Laser Settings")]
    [SerializeField] [Range(1, 8)] private int rayPerElement = 1;
    [SerializeField] [Range(0f, 10f)] private float maxChargeTime = 4f;
    [SerializeField] [Range(0f, 1000f)] private float maxDistance = 100f;
    [SerializeField] [Range(0f, 100f)] private float spendChance = 50f;
    private float chargeTime;
    private bool charging;
    private LightRay primaryRay;
    private LightRay[] adjacentRays;
    private float closestDist;

    protected override void Shoot(GameObject bullet)
    {
        AddAmmo();
        if (!OutOfAmmo())
        {
            if (Random.Range(0f, 100f) <= spendChance) SpendRandomAmmo();
            charging = true;
            chargeTime += Time.unscaledDeltaTime;
            float pow = Mathf.Clamp(chargeTime / maxChargeTime, 0f, 1f);
            primaryRay.SetGirth(pow / 3f + Mathf.Cos(Time.time * 32f * pow) / (32f + 32f * (1 - pow)));
            primaryRay.SetRay(bulletHole.position, bulletHole.forward, pow * Distance());
            for (int i = 0; i < rayPerElement * 3; ++i)
            {
                float offset = i * 360f / (rayPerElement * 3);
                Vector3 dir = Quaternion.AngleAxis(offset + pow * 360f, bulletHole.forward) * (bulletHole.forward * pow + (bulletHole.right + bulletHole.up).normalized * (1f - pow));
                adjacentRays[i].SetRay(bulletHole.position, dir, Mathf.Pow(pow, 2) * Distance());
                adjacentRays[i].SetGirth(pow / 10f);
            }
            HurtEnemiesInLine(pow);
        }
        Destroy(bullet);
    }

    protected override void SubStart()
    {
        reloadTime = 0f;
        primaryRay = Instantiate(lightRayPrefab, bulletHole).GetComponent<LightRay>();
        adjacentRays = new LightRay[rayPerElement * 3];
        for (int i = 0; i < rayPerElement * 3; ++i)
        {
            adjacentRays[i] = Instantiate(lightRayPrefab, bulletHole).GetComponent<LightRay>();
            if (i % 3 == 0) adjacentRays[i].SetMaterial(fireMat);
            else if (i % 3 == 1) adjacentRays[i].SetMaterial(waterMat);
            else adjacentRays[i].SetMaterial(teslaMat);
        }
        closestDist = 0f;
        RestRays();
    }

    protected override void SubUpdate()
    {
        if (charging) charging = false;
        else if (chargeTime > 0f) Release();
    }

    private void Release()
    {
        chargeTime = 0f;
        charging = false;
        RestRays();
    }

    private void RestRays()
    {
        primaryRay.SetGirth(0f);
        for (int i = 0; i < rayPerElement * 3; ++i) adjacentRays[i].SetGirth(0f);
    }

    private void HurtEnemiesInLine(float pow)
    {
        if (Physics.Raycast(bulletHole.position, bulletHole.forward * maxDistance * pow, out RaycastHit hit))
        {
            closestDist = Vector3.Distance(bulletHole.position, hit.point);
            int roll = Random.Range(0, 3);
            Ammo randomAmmo = (roll == 0? Ammo.Fire:(roll == 1? Ammo.Water:Ammo.Tesla));
            if (hit.collider.CompareTag("EnemyPart")) hit.collider.gameObject.GetComponent<BodyPart>().Hurt(hit.point, Damage(), randomAmmo);
        }
    }

    private float Distance() { return Mathf.Min(closestDist, maxDistance); }
}
