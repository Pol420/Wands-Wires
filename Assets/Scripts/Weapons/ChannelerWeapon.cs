using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelerWeapon : Weapon
{
    [Header("Physics")]
    [SerializeField] [Range(0f, 5f)] private float maximumChargeTime = 3f;
    [SerializeField] [Range(0f, 1f)] private float spellScale = 0.2f;
    private float charge;
    private bool charging;
    private GameObject currentBullet;
    private Vector3 startingScale;
    private Transform particles;



    protected override void Shoot(GameObject bullet)
    {
        AddAmmo();
        if (reloadTime == 0f)
        {
            charging = true;
            if (currentBullet == null)
            {
                currentBullet = bullet;
                particles = bullet.transform.GetChild(0);
                startingScale = particles.localScale;
                currentBullet.transform.forward = cam.transform.forward;
                cargaEv.setParameterByName("Shoot", 0);
                cargaEv.start();
            }
            else Destroy(bullet);
            currentBullet.transform.position = bulletHole.transform.position;
            particles.localScale = spellScale * startingScale * (charge / maximumChargeTime);
            if (charge >= maximumChargeTime) ShootCharge();
            else charge += Time.unscaledDeltaTime;
            anim.SetFloat("charge", charge / maximumChargeTime);
        }
        else Destroy(bullet);
    }

    private void ShootCharge()
    {
        anim.SetTrigger("Discharge");
        anim.SetFloat("charge", -1f);
        particles.localScale = startingScale;
        charge = Mathf.Min(maximumChargeTime, charge);
        float proportion = 2f * charge / maximumChargeTime;
        SpendAmmo(Mathf.RoundToInt(charge));
        currentBullet.GetComponent<Projectile>().ShootProjectile(bulletHole.position, cam.transform.forward, Damage() * proportion, ShotPower());
        charge = 0f;
        charging = false;
        currentBullet = null;
        reloadTime = 1f;
        cargaEv.setParameterByName("Shoot", 10);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Player/carga", GetComponent<Transform>().position);
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
