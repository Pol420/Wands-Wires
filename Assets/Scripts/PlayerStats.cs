using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Vector3Int ammo = new Vector3Int(99, 99, 99);
    [SerializeField] private HUD ammoHud = null;
    private static PlayerStats instance;

    private void Awake()
    {
        if (instance == null) instance = this;

    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
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
    public void SetFireAmmo(int amount) { ammo.x = Mathf.Clamp(amount, 0, 100); ammoHud.SetFire(ammo.x); }
    public void SetWaterAmmo(int amount) { ammo.y = Mathf.Clamp(amount, 0, 100); ammoHud.SetWater(ammo.y); }
    public void SetTeslaAmmo(int amount) { ammo.z = Mathf.Clamp(amount, 0, 100); ammoHud.SetTesla(ammo.z); }
    public int GetFireAmmo() { return ammo.x; }
    public int GetWaterAmmo() { return ammo.y; }
    public int GetTeslaAmmo() { return ammo.z; }
    public void SetHudSelector(float rate) { ammoHud.SetSelector(rate); }
    public void MoveHudSelector(int slot) { ammoHud.MoveSelector(slot); }
}
