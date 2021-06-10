using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PostProcessing;

public class PlayerPowers : MonoBehaviour
{
    private static PlayerPowers instance;
    public static PlayerPowers Instance() { return instance; }
    public static UnityEvent effectiveHit;
    private static ChromaticAberrationModel.Settings aberration;

    [Header("Power")]
    [SerializeField] [Range(0, 100)] private int slowMaxCharge = 50;
    [SerializeField] [Range(0f, 10f)] private float maxSloDuration = 5f;
    [SerializeField] [Range(0f, 1f)] private float slowAmount = 0.5f;
    private float slowmoDuration;
    private int slowCharge;

    [Header("Powerups")]
    [SerializeField] [Range(0f, 10f)] private float maxInvDuration = 6f;
    [SerializeField] [Range(0f, 10f)] private float maxInfDuration = 10f;
    [SerializeField] [Range(0f, 10f)] private float maxDeaDuration = 8f;
    [SerializeField] [Range(1f, 5f)] private float deadlyMultiplier = 4f;
    [SerializeField] [Range(0f, 0.5f)] private float killPowerIncrease = 0.05f;
    private float invincibleDuration;
    private float infiniteAmmoDuration;
    private float deadlyDuration;

    private HUD hud;
    public void SetHud(HUD hud) { this.hud = hud; }
    private int startingCharge;
    private Camera cam;
    private float startingLens;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            effectiveHit = new UnityEvent();
            effectiveHit.AddListener(OnEffectiveHit);
        }
    }

    void Start()
    {
        cam = Camera.main;
        startingLens = cam.focalLength;
        Enemy.death.AddListener(OnKill);
        InitPowers();
        ResetPowers();
        aberration = Camera.main.GetComponent<PostProcessingBehaviour>().profile.chromaticAberration.settings;
    }

    public void InitPowers()  { startingCharge = slowCharge; }

    public void ResetPowers()
    {
        invincibleDuration = Time.deltaTime;
        infiniteAmmoDuration = Time.deltaTime;
        deadlyDuration = Time.deltaTime;
        slowmoDuration = Time.deltaTime;
        slowCharge = startingCharge;
        SetSlowCharge();
    }

    void Update()
    {
        if (!LevelManager.paused)
        {
            if (Input.GetKeyDown(KeyCode.Z) && slowCharge >= slowMaxCharge) MakeSlowmo();
            CoolDownPowers();
            if (Slowmo())
            {
                Time.timeScale = 1f - slowAmount;
                cam.focalLength = startingLens / (1f + slowmoDuration / maxSloDuration);
                aberration.intensity = 1.1f - slowmoDuration / maxSloDuration;
            }
            else if (Time.timeScale != 1f)
            {
                Time.timeScale = 1f;
                cam.focalLength = startingLens;
            }
        }
    }

    private void CoolDownPowers()
    {
        if (Invincible()) { invincibleDuration -= Time.unscaledDeltaTime; hud.SetInvincible(invincibleDuration / maxInvDuration, Invincible()); }
        if (InfiniteAmmo()) { infiniteAmmoDuration -= Time.unscaledDeltaTime; hud.SetInfinite(infiniteAmmoDuration / maxInfDuration, InfiniteAmmo()); }
        if (Deadly()) { deadlyDuration -= Time.unscaledDeltaTime; hud.SetDeadly(deadlyDuration / maxInvDuration, Deadly()); }
        if (Slowmo()) { slowmoDuration -= Time.unscaledDeltaTime; hud.SetSlowmo(slowmoDuration / maxSloDuration, Slowmo()); }
    }

    private void OnKill()
    {
        if (Invincible()) invincibleDuration += maxInvDuration * killPowerIncrease;
        if (InfiniteAmmo()) infiniteAmmoDuration += maxInfDuration * killPowerIncrease;
        if (Deadly()) deadlyDuration += maxDeaDuration * killPowerIncrease;
        if (Slowmo()) slowmoDuration += maxSloDuration * killPowerIncrease;
    }
    
    private void OnEffectiveHit()
    {
        if (slowCharge < slowMaxCharge * 2)
        {
            slowCharge++;
            SetSlowCharge();
        }
    }

    public bool Invincible() { return invincibleDuration > 0f; }
    public bool InfiniteAmmo() { return infiniteAmmoDuration > 0f; }
    public bool Deadly() { return deadlyDuration > 0f; }
    private bool Slowmo() { return slowmoDuration > 0f; }
    public float GetDamageMultiplier() { return Deadly() ? deadlyMultiplier : 1f; }
    public void MakeInvincible() { invincibleDuration = maxInvDuration; }
    public void MakeInfinite() { infiniteAmmoDuration = maxInfDuration; }
    public void MakeDeadly() { deadlyDuration = maxDeaDuration; }
    public void MakeSlowmo() { slowmoDuration = maxSloDuration; slowCharge -= slowMaxCharge; SetSlowCharge(); }
    private void SetSlowCharge() { hud.SetSlowCharge((float)slowCharge / slowMaxCharge); }
}
