using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator), typeof(BodyPart))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject floatyTextPrefab = null;
    [SerializeField] private int health = 100;
    //[SerializeField] private Slider healthBar = null;
    private int currentHealth;
    private Animator anim;

    void Start()
    {
        GetComponent<BodyPart>().Connect(this, floatyTextPrefab);
        foreach (BodyPart part in transform.GetComponentsInChildren<BodyPart>()) part.Connect(this, floatyTextPrefab);
        currentHealth = health;
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        
    }

    public void Hurt(int amount)
    {
        currentHealth -= amount;
        //healthBar.value = currentHealth / health;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
