using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject otherWeaponObject = null;
    private Weapon otherWeapon;

    [SerializeField] protected static Vector3Int ammo = new Vector3Int(23, 23, 23);
    [SerializeField] protected Vector3 ammoReloadTime = new Vector3(1f, 2f, 3f);
    protected Transform bulletHole;
    private Ammo currentAmmo;
    protected float reloadTime;
    private Animator anim;
    protected Transform cam;

    void Start()
    {
        currentAmmo = Ammo.Fire;
        reloadTime = 0f;
        anim = GetComponent<Animator>();
        bulletHole = transform.GetChild(0);
        cam = Camera.main.transform;
        otherWeapon = otherWeaponObject.GetComponent<Weapon>();
        SubStart();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) SwitchWeapon();
        else if (reloadTime > 0f) reloadTime -= Time.deltaTime;
        else if (Input.GetButtonDown("Fire1"))
        {
            switch (currentAmmo)
            {
                case Ammo.Fire:
                    ShootFire();
                    ammo -= new Vector3Int(1, 0, 0);
                    reloadTime = ammoReloadTime.x;
                    //anim.SetTrigger("Shoot Fire");
                    break;
                case Ammo.Water:
                    ShootWater();
                    ammo -= new Vector3Int(0, 1, 0);
                    reloadTime = ammoReloadTime.y;
                    //anim.SetTrigger("Shoot Water");
                    break;
                case Ammo.Tesla:
                    ShootTesla();
                    ammo -= new Vector3Int(0, 0, 1);
                    reloadTime = ammoReloadTime.z;
                    //anim.SetTrigger("Shoot Tesla");
                    break;
                default:break;
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
                //anim.SetTrigger("Reload Fire");
                Debug.Log("Swtiched to Fire!");
                break;
            case Ammo.Water:
                reloadTime = ammoReloadTime.y;
                //anim.SetTrigger("Reload Water");
                Debug.Log("Swtiched to Water!");
                break;
            case Ammo.Tesla:
                reloadTime = ammoReloadTime.z;
                //anim.SetTrigger("Reload Tesla");
                Debug.Log("Swtiched to Tesla!");
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

}
public enum Ammo{Fire, Water, Tesla}
