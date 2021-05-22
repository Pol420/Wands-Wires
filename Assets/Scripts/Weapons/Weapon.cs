using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private bool unlocked = true;

    [Header("References")]
    [SerializeField] private GameObject otherWeaponObject = null;
    [SerializeField] private PlayerStats holder = null;
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
    [SerializeField] [Range(0f, 100f)] private float shotPower = 20f;
    [SerializeField] [Range(0f, 10f)] protected float weight = 0f;
    [SerializeField] [Range(0f, 100f)] private float damage = 20f;

    private Animator anim;
    protected static Transform cam;
    private LevelManager lm;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if(cam == null) cam = Camera.main.transform;
        otherWeapon = otherWeaponObject.GetComponent<Weapon>();
    }

    private void Start()
    {
        lm = LevelManager.Instance();
        SubStart();
    }

    void Update()
    {
        if (!LevelManager.paused)
        {
            if (Input.GetKeyDown(KeyCode.F)) SwitchWeapon();
            else if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchAmmo(Ammo.Fire);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchAmmo(Ammo.Water);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchAmmo(Ammo.Tesla);
            else if (currentReload > 0f)
            {
                currentReload -= Time.unscaledDeltaTime;
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
        gameObject.SetActive(false);
        otherWeapon.NextWeapon().SetActive(true);
    }
    public GameObject NextWeapon()
    {
        if (unlocked) return gameObject;
        if (otherWeapon == null) Awake();
        return otherWeapon.NextWeapon();
    }
    
    protected int GetCurrentAmmo() { return holder.GetCurrentAmmo(); }
    protected void SpendAmmo() { SpendAmmo(1); }
    protected void SpendAmmo(int amount) { holder.SpendAmmo(amount); }
    protected void AddAmmo() { SpendAmmo(-1); }
    protected float ShotPower() { return (1f / Time.timeScale) * shotPower; }
    protected float Damage() { return damage * holder.GetDamageMultiplier(); }

    protected abstract void Shoot(GameObject bullet);
    protected abstract void SubStart();
    protected abstract void SubUpdate();
    protected bool OutOfAmmo() { return holder.GetFireAmmo() * holder.GetWaterAmmo() * holder.GetTeslaAmmo() == 0; }
    protected void SpendRandomAmmo()
    {
        int roll = Random.Range(0, 3);
        if(roll == 0) holder.AddFireAmmo(-1);
        else if (roll == 1) holder.AddWaterAmmo(-1); 
        else holder.AddTeslaAmmo(-1);
    }
}
public enum Ammo{Fire, Water, Tesla}
