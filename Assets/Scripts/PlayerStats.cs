using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Vector3Int ammo = new Vector3Int(99, 99, 99);
    [SerializeField] private HUD hud = null;
    private static PlayerStats instance;

    [Header("Status")]
    [SerializeField] [Range(0f,500f)] private float maxHealth = 300f;
    [SerializeField] [Range(0f,500f)] private float maxShield = 300f;
    [SerializeField] [Range(0f, 1f)] private float shieldAbsorption = 0.75f;
    private float currentHealth;
    private float currentShield;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        SetHealth(maxHealth);
        SetShield(maxShield);
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.H)) Hurt(5f);
        if (Input.GetKey(KeyCode.V)) AddHealth(5f);
        if (Input.GetKey(KeyCode.E)) AddShield(1f);
    }

    public static PlayerStats Instance() { return instance; }

    public int GetCurrentAmmo(Ammo currentAmmo)
    {
        switch (currentAmmo)
        {
            case Ammo.Fire: return ammo.x;
            case Ammo.Water: return ammo.y;
            case Ammo.Tesla: return ammo.z;
            default: return 0;
        }
    }

    public void AddFireAmmo(int amount) { SetFireAmmo(ammo.x + amount); }
    public void AddWaterAmmo(int amount) { SetWaterAmmo(ammo.y + amount); }
    public void AddTeslaAmmo(int amount) { SetTeslaAmmo(ammo.z + amount); }
    public void SetFireAmmo(int amount) { ammo.x = Mathf.Clamp(amount, 0, 100); hud.SetFire(ammo.x); }
    public void SetWaterAmmo(int amount) { ammo.y = Mathf.Clamp(amount, 0, 100); hud.SetWater(ammo.y); }
    public void SetTeslaAmmo(int amount) { ammo.z = Mathf.Clamp(amount, 0, 100); hud.SetTesla(ammo.z); }
    public int GetFireAmmo() { return ammo.x; }
    public int GetWaterAmmo() { return ammo.y; }
    public int GetTeslaAmmo() { return ammo.z; }
    public void SetHudSelector(float rate) { hud.SetSelector(rate); }
    public void MoveHudSelector(int slot) { hud.MoveSelector(slot); }

    private void Hurt(float amount)
    {
        if (currentShield > 0f)
        {
            AddShield(-amount * shieldAbsorption);
            AddHealth(-amount * (1f - shieldAbsorption));
        }
        else AddHealth(-amount);
    }

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

}
