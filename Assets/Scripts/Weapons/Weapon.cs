using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject otherWeaponObject = null;
    [SerializeField] protected PlayerStats holder = null;
    [SerializeField] protected Transform bulletHole = null;
    private Weapon otherWeapon;

    [Header("Projectile Prefabs")]
    [SerializeField] protected GameObject fireBullet = null;
    [SerializeField] protected GameObject waterBullet = null;
    [SerializeField] protected GameObject teslaBullet = null;

    [Header("Ammo")]
    [SerializeField] [Range(0f, 5f)] protected float reloadTime = 0.5f;
    [SerializeField] private bool auto = false;
    private float currentReload;

    [Header("Stats")]
    [SerializeField] [Range(0f, 100f)] protected float shotPower = 20f;
    [SerializeField] [Range(0f, 10f)] protected float weight = 0f;
    [SerializeField] [Range(0f, 100f)] protected float damage = 20f;

    private Animator anim;
    protected Transform cam;

    void Awake()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        otherWeapon = otherWeaponObject.GetComponent<Weapon>();
        SubStart();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) SwitchWeapon();
        else if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchAmmo(Ammo.Fire);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchAmmo(Ammo.Water);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchAmmo(Ammo.Tesla);
        else if (currentReload > 0f)
        {
            currentReload -= Time.deltaTime;
            holder.SetHudSelector(1 - currentReload / reloadTime);
        }
        else if ((!auto && Input.GetButtonDown("Fire1")) || (auto && Input.GetButton("Fire1")))
        {
            if (holder.GetCurrentAmmo() > 0)
            {
                currentReload = reloadTime;
                Shoot(SpawnBullet());
                //todo trigger animation accordingly
            }
        }
        SubUpdate();
    }

    private GameObject SpawnBullet()
    {
        GameObject bullet;
        if (holder.GetCurrentAmmoType() == Ammo.Fire) bullet = Instantiate(fireBullet);
        else if (holder.GetCurrentAmmoType() == Ammo.Water) bullet = Instantiate(waterBullet);
        else bullet = Instantiate(teslaBullet);
        SpendAmmo();
        return bullet;
    }

    private void SwitchAmmo(Ammo ammoType)
    {
        holder.SwitchAmmo(ammoType);
        currentReload = reloadTime;
    }

    private void SwitchWeapon()
    {
        otherWeaponObject.SetActive(true);
        gameObject.SetActive(false);
    }
    
    protected int GetCurrentAmmo() { return holder.GetCurrentAmmo(); }
    protected void SpendAmmo() { SpendAmmo(1); }
    protected void SpendAmmo(int amount) { holder.SpendAmmo(amount); }
    protected void AddAmmo() { SpendAmmo(-1); }

    protected abstract void Shoot(GameObject bullet);
    protected abstract void SubStart();
    protected abstract void SubUpdate();
}
public enum Ammo{Fire, Water, Tesla}
