using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Vector3Int ammo = new Vector3Int(99, 99, 99);
    [SerializeField] private HUD hud = null;
    private static PlayerStats instance;
    public static UnityEvent death;
    public static UnityEvent effectiveHit;

    [Header("Status")]
    [SerializeField] [Range(0f,500f)] private float maxHealth = 300f;
    [SerializeField] [Range(0f,500f)] private float maxShield = 300f;
    [SerializeField] [Range(0f, 1f)] private float shieldAbsorption = 0.75f;
    [SerializeField] private Ammo currentAmmo;
    private float currentHealth;
    private float currentShield;

    [Header("Powers & Powerups")]
    [SerializeField] [Range(0, 100)] private int slowMaxCharge = 50;
    [SerializeField] [Range(0f, 10f)] private float maxSloDuration = 5f;
    [SerializeField] [Range(0f, 10f)] private float maxInvDuration = 6f;
    [SerializeField] [Range(0f, 10f)] private float maxInfDuration = 10f;
    [SerializeField] [Range(0f, 10f)] private float maxDeaDuration = 8f;
    [SerializeField] [Range(1f, 5f)] private float deadlyMultiplier = 4f;
    [SerializeField] [Range(0f, 1f)] private float slowAmount = 0.5f;
    [SerializeField] [Range(0f, 0.5f)] private float killPowerIncrease = 0.05f;

    private float invincibleDuration;
    private float infiniteAmmoDuration;
    private float deadlyDuration;
    private float slowmoDuration;
    private int slowCharge;

    private List<string> keyItems;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            death = new UnityEvent();
            death.AddListener(OnKill);
            effectiveHit = new UnityEvent();
            effectiveHit.AddListener(OnEffectiveHit);
        }
    }

    void Start()
    {
        SetHealth(maxHealth);
        SetShield(maxShield);
        SwitchAmmo(Ammo.Fire);
        AddFireAmmo(0);
        AddWaterAmmo(0);
        AddTeslaAmmo(0);
        invincibleDuration = Time.deltaTime;
        infiniteAmmoDuration = Time.deltaTime;
        deadlyDuration = Time.deltaTime;
        slowmoDuration = Time.deltaTime;
        keyItems = new List<string>();
        SetSlowCharge();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.H)) Hurt(5f);
        if (Input.GetKey(KeyCode.V)) AddHealth(5f);
        if (Input.GetKey(KeyCode.E)) AddShield(1f);
        if (Input.GetKey(KeyCode.Z) && slowCharge >= slowMaxCharge) MakeSlowmo();
        CoolDownPowers();
        if (Slowmo()) Time.timeScale = 1f - slowAmount;
        else if(Time.timeScale!= 1f) Time.timeScale = 1f;
    }

    private void CoolDownPowers() 
    {
        if (Invincible()) { invincibleDuration -= Time.unscaledDeltaTime; hud.SetInvincible(invincibleDuration / maxInvDuration, Invincible()); }
        if (InfiniteAmmo()) { infiniteAmmoDuration -= Time.unscaledDeltaTime; hud.SetInfinite(infiniteAmmoDuration / maxInfDuration, InfiniteAmmo()); }
        if (Deadly()) { deadlyDuration -= Time.unscaledDeltaTime; hud.SetDeadly(deadlyDuration / maxInvDuration, Deadly()); }
        if (Slowmo()) { slowmoDuration -= Time.unscaledDeltaTime; hud.SetSlowmo(slowmoDuration / maxSloDuration, Slowmo()); }
    }
    private bool Invincible() { return invincibleDuration > 0f; }
    private bool InfiniteAmmo() { return infiniteAmmoDuration > 0f; }
    private bool Deadly() { return deadlyDuration > 0f; }
    private bool Slowmo() { return slowmoDuration > 0f; }

    private void OnKill()
    {
        if (Invincible()) invincibleDuration += maxInvDuration * killPowerIncrease;
        if (InfiniteAmmo()) infiniteAmmoDuration += maxInfDuration * killPowerIncrease;
        if (Deadly()) deadlyDuration += maxDeaDuration * killPowerIncrease;
        if (Slowmo()) slowmoDuration += maxSloDuration * killPowerIncrease;
    }

    public static PlayerStats Instance() { return instance; }

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
        if (!InfiniteAmmo())
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

    public void AddFireAmmo(int amount) { SetFireAmmo(ammo.x + amount); }
    public void AddWaterAmmo(int amount) { SetWaterAmmo(ammo.y + amount); }
    public void AddTeslaAmmo(int amount) { SetTeslaAmmo(ammo.z + amount); }
    private void SetFireAmmo(int amount) { ammo.x = Mathf.Clamp(amount, 0, 256); hud.SetFire(ammo.x); }
    private void SetWaterAmmo(int amount) { ammo.y = Mathf.Clamp(amount, 0, 256); hud.SetWater(ammo.y); }
    private void SetTeslaAmmo(int amount) { ammo.z = Mathf.Clamp(amount, 0, 256); hud.SetTesla(ammo.z); }
    public int GetFireAmmo() { return ammo.x; }
    public int GetWaterAmmo() { return ammo.y; }
    public int GetTeslaAmmo() { return ammo.z; }
    public Ammo GetCurrentAmmoType() { return currentAmmo; }
    public void SetHudSelector(float rate) { hud.SetSelector(rate); }
    public void MoveHudSelector(int slot) { hud.MoveSelector(slot); }

    private void Hurt(float amount)
    {
        if (!Invincible())
        {
            if (currentShield > 0f)
            {
                AddShield(-amount * shieldAbsorption);
                AddHealth(-amount * (1f - shieldAbsorption));
            }
            else AddHealth(-amount);
        }
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

    public void MakeInvincible() { invincibleDuration = maxInvDuration; }
    public void MakeInfinite() { infiniteAmmoDuration = maxInfDuration; }
    public void MakeDeadly() { deadlyDuration = maxDeaDuration; }
    public void MakeSlowmo() { slowmoDuration = maxSloDuration; slowCharge -= slowMaxCharge; SetSlowCharge(); }

    public float GetDamageMultiplier() { return Deadly() ? deadlyMultiplier : 1f; }

    private void OnEffectiveHit()
    {
        if (slowCharge < slowMaxCharge * 2)
        {
            slowCharge++;
            SetSlowCharge();
        }
    }
    private void SetSlowCharge() { hud.SetSlowCharge((float)slowCharge / slowMaxCharge); }
}
