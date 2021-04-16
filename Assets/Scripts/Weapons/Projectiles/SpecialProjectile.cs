using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class SpecialProjectile : Projectile
{

    private bool spiral = false;
    private bool wobbly = false;

    private float power;
    private int spread;
    private float ttl;

    private void OnCollisionEnter(Collision collision) { Destroy(gameObject); }
    
    public void ShootSpiral(Vector3 position, Vector3 direction, float power, float offset)
    {
        body = GetComponent<Rigidbody>();
        transform.position = position;
        transform.forward = direction;
        transform.RotateAround(transform.position, transform.forward, offset * 360f);
        this.power = power;
        body.useGravity = false;
        spiral = true;
    }

    public void ShootWobbly(Vector3 position, Vector3 direction, float power, float ttl)
    {
        body = GetComponent<Rigidbody>();
        transform.position = position;
        transform.forward = direction;
        this.power = power;
        this.ttl = ttl;
        body.useGravity = false;
        wobbly = true;
    }

    private void Update()
    {
        if (spiral)
        {
            body.AddForce(transform.forward * power * Time.deltaTime * spread / 50f, ForceMode.Force);
            body.AddForce((transform.right - transform.up) * power * Time.deltaTime * spread / 5f, ForceMode.Force);
            transform.RotateAround(transform.position, transform.forward, 720f * Time.deltaTime);
        }
        else if (wobbly)
        {
            if (spread * Time.deltaTime > ttl) Destroy(gameObject);
            else
            {
                float randomicity = 2 + spread / 60f;
                Vector3 randomDirection = transform.forward + (Random.Range(0, randomicity) - randomicity / 2f) * transform.right + (Random.Range(0, randomicity) - randomicity / 2f) * transform.up;
                body.AddForce(randomDirection * power * Time.deltaTime * spread, ForceMode.Force);
                transform.RotateAround(transform.position, transform.forward, Random.Range(0f, 1440f) * Time.deltaTime);
            }
        }
        ++spread;
    }

    protected override void OnCollision(Collision collision)
    {
        // 
    }

    protected override void OnDrag(Collision collision)
    {
        //
    }
}
