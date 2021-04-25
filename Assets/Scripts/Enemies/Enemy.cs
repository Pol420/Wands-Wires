using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject floatyTextPrefab = null;
    [SerializeField] private int health = 100;
    [SerializeField] private Slider healthBar = null;
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
    }

    public void Hurt(int amount)
    {
        currentHealth -= amount;
        if (hb) healthBar.value = currentHealth / health;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        if (!dead)
        {
            dead = true;
            singularDeath.Invoke();
            Destroy(gameObject);
        }
    }
}
