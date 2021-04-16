using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownProjectile : Projectile
{
    [SerializeField] private GameObject explosionPrefab = null;
    private int wallBounces;
    private int enemyBounces;
    private float contactTime;
    private float explosionRadius;

    //Shoot with gravity
    public void Shoot(Vector3 position, Vector3 direction, float power, float weight) { Shoot(position, direction, power, weight, 0, 0, 0f); }
    public void Shoot(Vector3 position, Vector3 direction, float power, float weight, int bouncesInWalls, int bouncesInEnemies) { Shoot(position, direction, power, weight, 0, 0, 0f); }
    public void Shoot(Vector3 position, Vector3 direction, float power, float weight, int bouncesInWalls, int bouncesInEnemies, float radius)
    {
        body = GetComponent<Rigidbody>();
        transform.forward = direction;
        transform.position = position;
        body.SetDensity(weight);
        body.AddForce(direction * power * Time.deltaTime, ForceMode.Impulse);
        wallBounces = bouncesInWalls;
        enemyBounces = bouncesInEnemies;
        explosionRadius = radius;
    }

    //Shoot without gravity
    public void Shoot(Vector3 position, Vector3 direction, float power) { Shoot(position, direction, power, 0, 0); }
    public void Shoot(Vector3 position, Vector3 direction, float power, int bouncesInWalls, int bouncesInEnemies)
    {
        body = GetComponent<Rigidbody>();
        transform.forward = direction;
        transform.position = position;
        body.useGravity = false;
        body.AddForce(direction * power, ForceMode.Impulse);
        wallBounces = bouncesInWalls;
        enemyBounces = bouncesInEnemies;
        damage = (int) power;
    }

    private void OnDestroy()
    {
        if (explosionRadius > 0f)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach(Collider c in hits)
            {
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.localScale = explosionRadius * Vector3.one;
                explosion.transform.position = transform.position;
                BodyPart part = c.gameObject.GetComponent<BodyPart>();
                if (part != null) part.Hurt(c.ClosestPoint(transform.position), damage, type);
            }
        }
    }

    protected override void OnCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (enemyBounces > 0) enemyBounces--;
            else Destroy(gameObject);
        }
        else if (wallBounces > 0) wallBounces--;
        else Destroy(gameObject);
    }

    protected override void OnDrag(Collision collision)
    {
        if (contactTime >= 1f) Destroy(gameObject);
        else contactTime += Time.deltaTime;
    }
}
