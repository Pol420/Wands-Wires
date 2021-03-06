using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerPowers), typeof(PlayerAnimator))]
public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;
    public static PlayerStats Instance() { return instance; }

    [Header("References")]
    [SerializeField] private Vector3Int ammo = new Vector3Int(64, 64, 64);
    [SerializeField] private HUD hud = null;

    [Header("Status")]
    [SerializeField] [Range(0f,500f)] private float maxHealth = 300f;
    [SerializeField] [Range(0f,500f)] private float maxShield = 300f;
    [SerializeField] [Range(0f, 1f)] private float shieldAbsorption = 0.75f;
    [SerializeField] private Ammo currentAmmo = Ammo.Fire;

    [Header("Options")]
    [SerializeField] private bool healOnDeath = true;
    [SerializeField] private bool resetHealthOnLevelComplete = false;
    [SerializeField] private bool resetAmmoOnDeath = false;
    [SerializeField] private bool resetAmmoOnLevelComplete = false;

    private float currentHealth;
    private float currentShield;

    private List<string> keyItems;
    private PlayerPowers powers;
    private PlayerAnimator animator;
    private Weapon[] weapons;

    private Vector3Int startingAmmo;
    private float startingHealth;
    private float startingShield;
    private int currentWeapon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            powers = GetComponent<PlayerPowers>();
            animator = GetComponent<PlayerAnimator>();
            weapons = GetComponentsInChildren<Weapon>(true);
            powers.SetHud(hud);
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) { instance.transform.GetChild(0).position = transform.GetChild(0).position; Destroy(gameObject); }
        else InitPlayer();
    }

    void Start()
    {
        if (LevelManager.Instance().InMainMenu()) Destroy(gameObject);
        InitPlayer();
        ResetPlayer();
    }

    private void InitPlayer()
    {
        if (resetAmmoOnLevelComplete) startingAmmo = new Vector3Int(64, 64, 64);
        else startingAmmo = ammo;
        if (currentHealth > 0 && !resetHealthOnLevelComplete)
        {
            startingHealth = currentHealth;
            startingShield = currentShield;
        }
        else
        {
            startingHealth = maxHealth;
            startingShield = maxShield;
        }
        powers.InitPowers();
        animator.InitAnimator();
    }

    private void ResetPlayer()
    {
        if (healOnDeath)
        {
            SetHealth(maxHealth);
            SetShield(maxShield);
        }
        else
        {
            SetHealth(startingHealth);
            SetShield(startingShield);
        }
        SwitchAmmo(Ammo.Fire);
        if (resetAmmoOnDeath) ammo = new Vector3Int(99, 99, 99);
        else ammo = startingAmmo;
        AddFireAmmo(0);
        AddWaterAmmo(0);
        AddTeslaAmmo(0);
        keyItems = new List<string>();
        powers.ResetPowers();
        hud.ResetHud();
        animator.InitAnimator();
    }

    public int GetCurrentAmmo()
    {
        switch (currentAmmo)
        {
            case Ammo.Fire: return ammo.x;
            case Ammo.Water: return ammo.y;
            case Ammo.Tesla: return ammo.z;
            default: return 0;
        }
    }

    public void SpendAmmo(int amount)
    {
        if (!powers.InfiniteAmmo())
        {
            if (currentAmmo == Ammo.Fire) AddFireAmmo(-amount);
            else if (currentAmmo == Ammo.Water) AddWaterAmmo(-amount);
            else AddTeslaAmmo(-amount);
        }
    }

    public void SwitchAmmo(Ammo ammoType)
    {
        currentAmmo = ammoType;
        PlayerAnimator.Reload();
        switch (ammoType)
        {
            case Ammo.Fire:
                MoveHudSelector(0);
                break;
            case Ammo.Water:
                MoveHudSelector(1);
                break;
            case Ammo.Tesla:
                MoveHudSelector(2);
                break;
            default: break;
        }
    }

    public void Hurt(float amount)
    {
        if (!powers.Invincible())
        {
            if (currentShield > 0f)
            {
                AddShield(-amount * shieldAbsorption);
                AddHealth(-amount * (1f - shieldAbsorption));
            }
            else AddHealth(-amount);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/damage", GetComponent<Transform>().position);
        }
    }

    private void SetFireAmmo(int amount) { ammo.x = Mathf.Clamp(amount, 0, 256); hud.SetFire(ammo.x); }
    private void SetWaterAmmo(int amount) { ammo.y = Mathf.Clamp(amount, 0, 256); hud.SetWater(ammo.y); }
    private void SetTeslaAmmo(int amount) { ammo.z = Mathf.Clamp(amount, 0, 256); hud.SetTesla(ammo.z); }

    public void AddFireAmmo(int amount) { if(!(amount < 0f && powers.InfiniteAmmo())) SetFireAmmo(ammo.x + amount); }
    public void AddWaterAmmo(int amount) { if (!(amount < 0f && powers.InfiniteAmmo())) SetWaterAmmo(ammo.y + amount); }
    public void AddTeslaAmmo(int amount) { if (!(amount < 0f && powers.InfiniteAmmo())) SetTeslaAmmo(ammo.z + amount); }
    public int GetFireAmmo() { return ammo.x; }
    public int GetWaterAmmo() { return ammo.y; }
    public int GetTeslaAmmo() { return ammo.z; }
    public Ammo GetCurrentAmmoType() { return currentAmmo; }
    public void SetHudSelector(float rate) { hud.SetSelector(rate); }
    public void MoveHudSelector(int slot) { hud.MoveSelector(slot); }
    public float GetDamageMultiplier() { return powers.GetDamageMultiplier(); }

    public void AddHealth(float amount) { SetHealth(Mathf.Clamp(currentHealth + amount, 0f, maxHealth)); }
    public void SetHealth(float amount)
    {
        currentHealth = amount;
        hud.SetHealth(currentHealth / maxHealth);
        if(currentHealth <= 0f) Die();
    }

    private void Die() { LevelManager.Instance().ReloadScene(); ResetPlayer(); }

    public void AddShield(float amount) { SetShield(Mathf.Clamp(currentShield + amount, 0f, maxShield)); }
    public void SetShield(float amount)
    {
        currentShield = amount;
        hud.SetShield(currentShield / maxShield);
    }

    public void AddKeyItem(string code, GameObject key)
    {
        keyItems.Add(code);
        hud.AddKeyItem(key);
    }
    private void RemoveKeyItem(string code)
    {
        keyItems.Remove(code);
        hud.RemoveKeyItem(code);
    }

    public bool GetKey(string code)
    {
        if (keyItems.Contains(code))
        {
            RemoveKeyItem(code);
            return true;
        }
        return false;
    }

    public void UnlockWeapon(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Wand: SwitchWeapons(0); break;
            case WeaponType.Staff: SwitchWeapons(1); break;
            case WeaponType.Grimoir: SwitchWeapons(2); break;
            case WeaponType.Prism: SwitchWeapons(3); break;
            default: break;
        }
    }
    private void SwitchWeapons(int target)
    {
        currentWeapon = target;
        for (int i = 0; i < weapons.Length; ++i)
        {
            if (i == target)
            {
                weapons[i].gameObject.SetActive(true);
                weapons[i].Unlock();
            }
            else weapons[i].gameObject.SetActive(false);
        }
    }

    public void Kill() { Die(); }
}
