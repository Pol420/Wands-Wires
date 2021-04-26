using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageDetection : MonoBehaviour
{
    private PlayerStats playerStats;
    private CapsuleCollider collider;
    
    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.Instance();
        collider = GetComponent<CapsuleCollider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag("EnemyProjectile"))
            playerStats.Hurt(other.transform.GetComponent<Enemy_Projectile>().GetDamage());    
    }
}
