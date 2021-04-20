using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile_Normal : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float damage = 20.0f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = speed * transform.forward;
    }
}
