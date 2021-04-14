using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private int wallBounces;
    private int enemyBounces;
    private float contactTime;
    private bool spiral = false;
    
    private Rigidbody body;
    private float power;
    private int spread;


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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //damage enemy or smth
            if (enemyBounces > 0) enemyBounces--;
            else Destroy(gameObject);
        }
        else if (wallBounces > 0) wallBounces--;
        else Destroy(gameObject);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (contactTime >= 1f) Destroy(gameObject);
        else contactTime += Time.deltaTime;
    }
    
    public void ShootSpiral(Vector3 position, Vector3 direction, float power, float offset)
    {
        body = GetComponent<Rigidbody>();
        transform.position = position;
        transform.forward = direction;
        transform.RotateAround(transform.position, transform.forward, offset * 360f);
        body.useGravity = false;
        spiral = true;
        this.power = power;
    }

    private void Update()
    {
        if (spiral)
        {
            body.AddForce(transform.forward * power * Time.deltaTime * spread / 10f, ForceMode.Force);
            body.AddForce((transform.right - transform.up) * spread * power * Time.deltaTime, ForceMode.Force);
            transform.RotateAround(transform.position, transform.forward, 720f * Time.deltaTime);
            ++spread;
        }
    }

}
