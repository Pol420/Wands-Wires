using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject floatyTextPrefab = null;
    [SerializeField] private Slider healthBar = null;
    [SerializeField] private Material[] mats = new Material[] { null, null, null };

    [Header("Stats")]
    [SerializeField] private int health = 100;
    [SerializeField] protected Ammo type = Ammo.Fire;
    private int currentHealth;

    [Header("Drops")]
    [SerializeField] [Range(0f, 100f)] private float dropChance = 100f;
    [SerializeField] [Range(0f, 100f)] private float ammoToStatusChance = 75f;
    [SerializeField] [Range(0f, 100f)] private float powerupDropChance = 10f;

    private Animator anim;
    private bool hb;
    public static UnityEvent death;
    public UnityEvent singularDeath;
    private bool dead;
    private bool damaged;


    //Provisional
    private EnemyBehaviourNormal isNormal;

    private void Awake()
    {
        if(death == null) death = new UnityEvent();
        singularDeath = new UnityEvent();
        singularDeath.AddListener(death.Invoke);
        dead = false;
        damaged = false;
        isNormal = GetComponent<EnemyBehaviourNormal>();
    }

    void Start()
    {
        foreach (BodyPart part in transform.GetComponentsInChildren<BodyPart>()) part.Connect(this, floatyTextPrefab, type);
        currentHealth = health;
        anim = GetComponent<Animator>();
        hb = healthBar != null;
        Renderer rend = GetComponent<Renderer>();
        if(rend == null) rend = GetComponentInChildren<SkinnedMeshRenderer>();
        switch(type)
        {
            case Ammo.Fire: rend.material = mats[0]; break;
            case Ammo.Water: rend.material = mats[1]; break;
            case Ammo.Tesla: rend.material = mats[2]; break;
            default: break;
        }
    }

    public void Hurt(int amount)
    {
        currentHealth -= amount;
        if (hb) healthBar.value = currentHealth / health;
        if (!dead)
        {
            if (currentHealth <= 0)
            {
                damaged = false;
                Die();
            }
            else
            {
                anim.SetTrigger("Damaged");
                damaged = true; 
            }
        }
    }

    private void Die()
    {
        if (!dead)
        {
            dead = true;
            anim.SetBool("Dead", true);
            singularDeath.Invoke();
            if(isNormal != null)
                anim.SetTrigger("Die");
            else
            {
                DestroyEnemy();
            }
            //DestroyEnemy(); //temp before animation events!
        }
    }

    public void DestroyEnemy()
    {
        if (Random.Range(0f, 1f) <= dropChance) DropItem();
        Destroy(gameObject);
    }

    private void DropItem()
    {
        GameObject drop;
        if(Roll(powerupDropChance)) drop = Powerup.RandomPowerup(transform.position);
        else  if (Roll(ammoToStatusChance)) drop = StatusPickup.DropAmmo(transform.position, type);
        else drop = StatusPickup.RandomStatus(transform.position);
        drop.transform.position = transform.position;
    }

    private bool Roll(float threshold) { return Random.Range(0f, 100f) <= threshold; }

    public bool IsDead()
    {
        return dead;
    }

    public void NotDamaged()
    {
        damaged = false;
    }

    public bool IsDamaged()
    {
        return damaged;
    }
}
