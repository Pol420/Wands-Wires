using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageDetection : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerStats.Instance();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("EnemyProjectile")) 
            playerStats.Hurt(other.transform.GetComponent<Enemy_Projectile>().GetDamage());
        else if (other.transform.CompareTag("EnemyAttack")) 
            playerStats.Hurt(other.transform.parent.GetComponent<Enemy_Data>().GetAttackDamage());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("EnemyProjectile")) 
            playerStats.Hurt(other.transform.GetComponent<Enemy_Projectile>().GetDamage());
        else if (other.transform.CompareTag("EnemyAttack")) 
            playerStats.Hurt(other.transform.parent.GetComponent<Enemy_Data>().GetAttackDamage());
    }
}
