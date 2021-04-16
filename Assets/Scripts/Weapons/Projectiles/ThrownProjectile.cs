using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownProjectile : Projectile
{

    private int wallBounces;
    private int enemyBounces;
    private float contactTime;

    //Shoot with gravity
    public void Shoot(Vector3 position, Vector3 direction, float power, float weight) { Shoot(position, direction, power, weight, 0, 0); }
    public void Shoot(Vector3 position, Vector3 direction, float power, float weight, int bouncesInWalls, int bouncesInEnemies)
    {
        body = GetComponent<Rigidbody>();
        transform.forward = direction;
        transform.position = position;
        body.SetDensity(weight);
        body.AddForce(direction * power * Time.deltaTime, ForceMode.Impulse);
        wallBounces = bouncesInWalls;
        enemyBounces = bouncesInEnemies;
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
