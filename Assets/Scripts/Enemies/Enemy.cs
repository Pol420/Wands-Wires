using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject floatyTextPrefab = null;
    [SerializeField] private Slider healthBar = null;
    [SerializeField] private StatusPickup[] pickups; //TODO weighed drops
    [SerializeField] private Material[] mats = new Material[] { null, null, null };

    [Header("Stats")]
    [SerializeField] private int health = 100;
    [SerializeField] protected Ammo type = Ammo.Fire;
    private int currentHealth;
    private Animator anim;
    private bool hb;
    public static UnityEvent death;
    public UnityEvent singularDeath;
    private bool dead;

    private void Awake()
    {
        if(death == null) death = new UnityEvent();
        singularDeath = new UnityEvent();
        singularDeath.AddListener(death.Invoke);
        dead = false;
    }

    void Start()
    {
        foreach (BodyPart part in transform.GetComponentsInChildren<BodyPart>()) part.Connect(this, floatyTextPrefab, type);
        currentHealth = health;
        anim = GetComponent<Animator>();
        hb = healthBar != null;
        Renderer rend = GetComponent<Renderer>();
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
        if (currentHealth <= 0 && !dead) Die();
    }

    private void Die()
    {
        if (!dead)
        {
            dead = true;
            LeavePickUp();
            singularDeath.Invoke();
            Destroy(gameObject);
        }
    }

    private void LeavePickUp()
    {
        if (pickups.Length > 0)
        {
            Instantiate(pickups[0], transform.position + Vector3.up * 2, transform.rotation);
        }
    }
}
