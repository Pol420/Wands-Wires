using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float damage = 20.0f;
    [SerializeField] private float timeToDestroy = 5.0f;
    private Rigidbody rb;


    private void Awake() { Destroy(gameObject, timeToDestroy); }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = speed * transform.forward;
    }

    private void OnTriggerEnter(Collider other) { Destroy(gameObject); }

    public float GetDamage()
    {
        return damage;
    }
}
