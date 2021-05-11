using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourSpawner : EnemyBehaviour
{
    [SerializeField] private List<GameObject> enemiesToSpawn = new List<GameObject>();

    public override void Attack()
    {
        LookAtPlayer();

        if (InComfortZone(transform.position)) SpawnEnemy();
        else AvoidPlayer();
    }

    private void SpawnEnemy()
    {
        if (timeToAttack <= 0)
        {
            Instantiate(GetEnemyToSpawn(), shootingPoint.transform.position, shootingPoint.transform.rotation, transform.parent);
            timeToAttack = enemyData.GetMaxTimeToAttack();
        }
        else timeToAttack -= Time.deltaTime;
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

    private GameObject GetEnemyToSpawn()
    {
        if (enemiesToSpawn.Count > 0) return enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];
        else
        {
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (!enemy.GetComponent<EnemyBehaviourSpawner>()) return enemy;
            }
        }
        return null;
    }
}
