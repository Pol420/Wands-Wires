using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemyBehaviourSpawner : EnemyBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;

    private void Awake()
    {
        if (enemyToSpawn == null)
            enemyToSpawn = DefaultEnemyToSpawn();
    }

    public override void Attack()
    {
        LookAtPlayer();

        if (InComfortZone(transform.position))
        {
            SpawnEnemy();
        }
            
        else
            AvoidPlayer();
    }

    private void SpawnEnemy()
    {
        if (timeToAttack <= 0)
        {
            Instantiate(enemyToSpawn, shootingPoint.transform.position, shootingPoint.transform.rotation);
            timeToAttack = enemyData.GetMaxTimeToAttack();
        }
        else
        {
            timeToAttack -= Time.deltaTime;
        }
    }

    protected override bool InComfortZone(Vector3 position)
    {
        return CalculateDistance(player.position, position) <= enemyData.GetAttackDistance() && 
               CalculateDistance(player.position, position) >= enemyData.GetAttackDistance() - 10.0f;;
    }
    
    protected override void AvoidPlayer()
    {
        if (CalculateDistance(navMeshAgent.destination, transform.position) < errorDistance)
        {
            Vector3 newPosition = transform.position + transform.forward * - 
                (enemyData.GetAttackDistance() - CalculateDistance(player.position, transform.position));

            if (!ValidPosition(newPosition))
            {
                newPosition = transform.position + transform.forward *  
                    (enemyData.GetAttackDistance() + CalculateDistance(player.position, transform.position));

                if (!ValidPosition(newPosition))
                {
                    newPosition = transform.position + transform.right *  
                        (enemyData.GetAttackDistance() - errorDistance);
                    if (!ValidPosition(newPosition))
                    {
                        newPosition = transform.position + transform.right *  
                            - (enemyData.GetAttackDistance() - errorDistance);
                        
                    }
    
                }
            }

            navMeshAgent.destination = newPosition;
        }
    }

    private GameObject DefaultEnemyToSpawn()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            if(!enemy.GetComponent<EnemyBehaviourSpawner>())
                return enemy;  
        }

        return gameObject;
    }
}
