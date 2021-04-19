using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private GameObject otherWeaponObject = null;
    [SerializeField] protected Pushable holder = null;
    [SerializeField] private HUD hud = null;
    //private Weapon otherWeapon;

    [Header("Projectile Prefabs")]
    [SerializeField] protected GameObject fireBullet = null;
    [SerializeField] protected GameObject waterBullet = null;
    [SerializeField] protected GameObject teslaBullet = null;

    [Header("Ammo")]
    [SerializeField] private Vector3Int ammo = new Vector3Int(23, 23, 23);
    [SerializeField] [Range(0f, 5f)] protected float reloadTime = 0.5f;
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
        AddFireAmmo(0);
        AddWaterAmmo(0);
        AddTeslaAmmo(0);
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
            hud.SetSelector(1 - currentReload / reloadTime);
        }
        else if (Input.GetButton("Fire1"))
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
        SubUpdate();
    }

    private void SwitchAmmo(Ammo ammoType)
    {
        currentAmmo = ammoType;
        currentReload = reloadTime;
        switch (ammoType)
        {
            case Ammo.Fire:
                hud.MoveSelector(0);
                //anim.SetTrigger("Reload Fire");
                break;
            case Ammo.Water:
                hud.MoveSelector(1);
                //anim.SetTrigger("Reload Water");
                break;
            case Ammo.Tesla:
                hud.MoveSelector(2);
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

    protected abstract void Shoot(Ammo ammo);
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
