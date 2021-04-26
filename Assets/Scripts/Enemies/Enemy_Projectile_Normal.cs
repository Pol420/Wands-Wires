using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile_Normal : MonoBehaviour, Enemy_Projectile
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float damage = 20.0f;
    [SerializeField] private float timeToDestroy = 5.0f;
    private Rigidbody rb;


    private void Awake()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = speed * transform.forward;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag("Player"))
            Destroy(gameObject);
    }

    public float GetDamage()
    {
        return damage;
    }
}
