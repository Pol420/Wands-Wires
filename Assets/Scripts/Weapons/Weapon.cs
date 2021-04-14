using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject otherWeaponObject = null;
    private Weapon otherWeapon;
    [SerializeField] private HUD hud = null;

    [SerializeField] private static Vector3Int ammo = new Vector3Int(23, 23, 23);
    [SerializeField] protected Vector3 ammoReloadTime = new Vector3(1f, 2f, 3f);
    protected Transform bulletHole;
    private Ammo currentAmmo;
    protected float reloadTime;
    private Animator anim;
    protected Transform cam;

    void Start()
    {
        anim = GetComponent<Animator>();
        bulletHole = transform.GetChild(0);
        cam = Camera.main.transform;
        otherWeapon = otherWeaponObject.GetComponent<Weapon>();
        AddFireAmmo(0);
        AddWaterAmmo(0);
        AddTeslaAmmo(0);
        SwitchAmmo(Ammo.Fire);
        SubStart();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) SwitchWeapon();
        else if (reloadTime > 0f)
        {
            reloadTime -= Time.deltaTime;
            hud.SetSelector(1 - reloadTime / ammoReloadTime[CurrentAmmo()]);
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            switch (currentAmmo)
            {
                case Ammo.Fire:
                    ShootFire();
                    reloadTime = ammoReloadTime.x;
                    //anim.SetTrigger("Shoot Fire");
                    break;
                case Ammo.Water:
                    ShootWater();
                    reloadTime = ammoReloadTime.y;
                    //anim.SetTrigger("Shoot Water");
                    break;
                case Ammo.Tesla:
                    ShootTesla();
                    reloadTime = ammoReloadTime.z;
                    //anim.SetTrigger("Shoot Tesla");
                    break;
                default: break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchAmmo(Ammo.Fire);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchAmmo(Ammo.Water);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchAmmo(Ammo.Tesla);
        SubUpdate();
    }

    private void SwitchAmmo(Ammo ammoType)
    {
        currentAmmo = ammoType;
        switch (ammoType)
        {
            case Ammo.Fire:
                reloadTime = ammoReloadTime.x;
                hud.MoveSelector(0);
                //anim.SetTrigger("Reload Fire");
                break;
            case Ammo.Water:
                reloadTime = ammoReloadTime.y;
                hud.MoveSelector(1);
                //anim.SetTrigger("Reload Water");
                break;
            case Ammo.Tesla:
                reloadTime = ammoReloadTime.z;
                hud.MoveSelector(2);
                //anim.SetTrigger("Reload Tesla");
                break;
            default: break;
        }
    }

    private void SwitchWeapon()
    {
        otherWeaponObject.SetActive(true);
        otherWeapon.SwitchAmmo(currentAmmo);
        gameObject.SetActive(false);
    }

    protected abstract void ShootFire();
    protected abstract void ShootWater();
    protected abstract void ShootTesla();
    protected abstract void SubStart();
    protected abstract void SubUpdate();

    protected void AddFireAmmo(int amount) { SetFireAmmo(ammo.x + amount); }
    protected void AddWaterAmmo(int amount) { SetWaterAmmo(ammo.y + amount); }
    protected void AddTeslaAmmo(int amount) { SetTeslaAmmo(ammo.z + amount); }
    protected void SetFireAmmo(int amount) { ammo.x = Mathf.Clamp(amount, 0, 100); hud.SetFire(ammo.x); }
    protected void SetWaterAmmo(int amount) { ammo.y = Mathf.Clamp(amount, 0, 100); hud.SetWater(ammo.y); }
    protected void SetTeslaAmmo(int amount) { ammo.z = Mathf.Clamp(amount, 0, 100); hud.SetTesla(ammo.z); }
    protected int GetFireAmmo() { return ammo.x; }
    protected int GetWaterAmmo() { return ammo.y; }
    protected int GetTeslaAmmo() { return ammo.z; }
    protected int CurrentAmmo() { if (currentAmmo == Ammo.Fire) return 0; else if (currentAmmo == Ammo.Water) return 1; else return 2; }

}
public enum Ammo{Fire, Water, Tesla}
