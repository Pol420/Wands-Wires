using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageDetection : MonoBehaviour
{
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.Instance();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("EnemyProjectile"))
        {
            playerStats.Hurt(other.transform.GetComponent<Enemy_Projectile>().GetDamage());
        }
                
    }
}
