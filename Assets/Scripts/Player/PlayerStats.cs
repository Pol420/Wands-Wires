using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerPowers))]
public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;
    public static PlayerStats Instance() { return instance; }

    [Header("References")]
    [SerializeField] private Vector3Int ammo = new Vector3Int(99, 99, 99);
    [SerializeField] private HUD hud = null;

    [Header("Status")]
    [SerializeField] [Range(0f,500f)] private float maxHealth = 300f;
    [SerializeField] [Range(0f,500f)] private float maxShield = 300f;
    [SerializeField] [Range(0f, 1f)] private float shieldAbsorption = 0.75f;
    [SerializeField] private Ammo currentAmmo;
    private float currentHealth;
    private float currentShield;

    private List<string> keyItems;
    private PlayerPowers powers;

    private void Awake() { if (instance == null) instance = this; }

    void Start()
    {
        SetHealth(maxHealth);
        SetShield(maxShield);
        SwitchAmmo(Ammo.Fire);
        AddFireAmmo(0);
        AddWaterAmmo(0);
        AddTeslaAmmo(0);
        keyItems = new List<string>();
        powers = GetComponent<PlayerPowers>();
        powers.SetHud(hud);
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.H)) Hurt(5f);
        if (Input.GetKey(KeyCode.V)) AddHealth(5f);
        if (Input.GetKey(KeyCode.E)) AddShield(1f);
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
        switch (ammoType)
        {
            case Ammo.Fire:
                MoveHudSelector(0);
                //anim.SetTrigger("Reload Fire");
                break;
            case Ammo.Water:
                MoveHudSelector(1);
                //anim.SetTrigger("Reload Water");
                break;
            case Ammo.Tesla:
                MoveHudSelector(2);
                //anim.SetTrigger("Reload Tesla");
                break;
            default: break;
        }
    }

    private void Hurt(float amount)
    {
        if (!powers.Invincible())
        {
            if (currentShield > 0f)
            {
                AddShield(-amount * shieldAbsorption);
                AddHealth(-amount * (1f - shieldAbsorption));
            }
            else AddHealth(-amount);
        }
    }

    private void SetFireAmmo(int amount) { ammo.x = Mathf.Clamp(amount, 0, 256); hud.SetFire(ammo.x); }
    private void SetWaterAmmo(int amount) { ammo.y = Mathf.Clamp(amount, 0, 256); hud.SetWater(ammo.y); }
    private void SetTeslaAmmo(int amount) { ammo.z = Mathf.Clamp(amount, 0, 256); hud.SetTesla(ammo.z); }

    public void AddFireAmmo(int amount) { SetFireAmmo(ammo.x + amount); }
    public void AddWaterAmmo(int amount) { SetWaterAmmo(ammo.y + amount); }
    public void AddTeslaAmmo(int amount) { SetTeslaAmmo(ammo.z + amount); }
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
    }
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
}
