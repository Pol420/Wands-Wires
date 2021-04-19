using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] public Ammo type = Ammo.Fire;
    public float damage;
    protected Rigidbody body;

    public void ShootProjectile(Vector3 position, Vector3 direction, float bulletDamage, float power) { ShootProjectile(position, direction, bulletDamage, power, 0f); }
    public void ShootProjectile(Vector3 position, Vector3 direction, float bulletDamage, float power, float weight)
    {
        body = GetComponent<Rigidbody>();
        damage = bulletDamage;
        body.mass = weight;
        body.useGravity = weight > 0f;
        Shoot(position, direction, power);
    } 

    private void OnCollisionEnter(Collision collision) { Explode(); }
    protected abstract void Shoot(Vector3 position, Vector3 direction, float power);
    protected abstract void Explode();

    public void DestroyProjectile() { Destroy(gameObject); }
}
