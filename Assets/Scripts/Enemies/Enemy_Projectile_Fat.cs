using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile_Fat : MonoBehaviour, Enemy_Projectile
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
        GiveInitialSpeed();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
            Destroy(gameObject);
        else if(other.transform.CompareTag("Untagged"))
            Destroy(gameObject);
    }
    /*private void OnTriggerEnter()
    {
        if(other.transform.CompareTag("Player"))
            Destroy(gameObject);
        
    }*/

    private void GiveInitialSpeed()
    {
        rb.velocity = speed * transform.forward;
    }

    public float GetDamage()
    {
        return damage;
    }
}
