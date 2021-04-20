using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private GameObject otherWeaponObject = null;
    [SerializeField] protected PlayerStats holder = null;
    //private Weapon otherWeapon;

    [Header("Projectile Prefabs")]
    [SerializeField] protected GameObject fireBullet = null;
    [SerializeField] protected GameObject waterBullet = null;
    [SerializeField] protected GameObject teslaBullet = null;

    [Header("Ammo")]
    [SerializeField] [Range(0f, 5f)] protected float reloadTime = 0.5f;
    [SerializeField] private bool auto = false;
    private float currentReload;

    private Ammo currentAmmo;
    private Animator anim;
    protected Transform cam;
    protected Transform bulletHole; //must have an empty object as first child

    void Awake()
    {
        anim = GetComponent<Animator>();
        bulletHole = transform.GetChild(0);
        cam = Camera.main.transform;
        //otherWeapon = otherWeaponObject.GetComponent<Weapon>();
        holder.AddFireAmmo(0);
        holder.AddWaterAmmo(0);
        holder.AddTeslaAmmo(0);
        SwitchAmmo(Ammo.Fire);
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
            if (holder.GetCurrentAmmo(currentAmmo) > 0)
            {
                currentReload = reloadTime;
                switch (currentAmmo)
                {
                    case Ammo.Fire:
                        Shoot(Ammo.Fire);
                        //anim.SetTrigger("Shoot Fire");
                        break;
                    case Ammo.Water:
                        Shoot(Ammo.Water);
                        //anim.SetTrigger("Shoot Water");
                        break;
                    case Ammo.Tesla:
                        Shoot(Ammo.Tesla);
                        //anim.SetTrigger("Shoot Tesla");
                        break;
                    default: break;
                }
            }
        }
        SubUpdate();
    }

    private void SwitchAmmo(Ammo ammoType)
    {
        currentAmmo = ammoType;
        currentReload = reloadTime;
        switch (ammoType)
        {
            case Ammo.Fire:
                holder.MoveHudSelector(0);
                //anim.SetTrigger("Reload Fire");
                break;
            case Ammo.Water:
                holder.MoveHudSelector(1);
                //anim.SetTrigger("Reload Water");
                break;
            case Ammo.Tesla:
                holder.MoveHudSelector(2);
                //anim.SetTrigger("Reload Tesla");
                break;
            default: break;
        }
    }

    private void SwitchWeapon()
    {
        //otherWeaponObject.SetActive(true);
        //otherWeapon.SwitchAmmo(currentAmmo);
        //gameObject.SetActive(false);
    }

    protected void SpendFire() { SpendFire(1); }
    protected void SpendFire(int amount) { holder.AddFireAmmo(-amount); }
    protected void SpendWater() { SpendWater(1); }
    protected void SpendWater(int amount) { holder.AddWaterAmmo(-amount); }
    protected void SpendTesla() { SpendTesla(1); }
    protected void SpendTesla(int amount) { holder.AddTeslaAmmo(-amount); }
    protected int GetCurrentAmmo() { return holder.GetCurrentAmmo(currentAmmo); }

    protected abstract void Shoot(Ammo ammo);
    protected abstract void SubStart();
    protected abstract void SubUpdate();
}
public enum Ammo{Fire, Water, Tesla}
