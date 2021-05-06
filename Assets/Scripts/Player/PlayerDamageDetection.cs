using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageDetection : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Start() { playerStats = PlayerStats.Instance(); }
    private void OnTriggerEnter(Collider other) { Collide(other.gameObject); }
    private void OnCollisionEnter(Collision other) { Collide(other.gameObject); }

    private void Collide(GameObject other)
    {
        if (other.CompareTag("EnemyProjectile")) playerStats.Hurt(other.GetComponent<Enemy_Projectile>().GetDamage());
        else if (other.CompareTag("EnemyAttack")) playerStats.Hurt(other.transform.parent.GetComponent<Enemy_Data>().GetAttackDamage());
    }
}
