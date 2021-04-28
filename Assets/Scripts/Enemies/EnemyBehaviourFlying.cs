using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourFlying : EnemyBehaviour
{
    public override void Attack()
    {
        LookAtPlayer();
        
        ShootingAttack();

        if (InComfortZone(transform.position))
        {
            MovementAttack();
        }
            
        else
            AvoidPlayer();
    }
    
    private void ShootingAttack()
    {
        if (timeToShoot <= 0)
        {
            Instantiate(enemyData.GetProjectile(), shootingPoint.transform.position, shootingPoint.transform.rotation);
            timeToShoot = enemyData.GetMaxTimeToShoot();
        }
        else
        {
            timeToShoot -= Time.deltaTime;
        }
    }

    private void MovementAttack()
    {
        if (CalculateDistance(navMeshAgent.destination, transform.position) < errorDistance)
        {
            Vector3 newPosition = transform.position + 
                                  new Vector3(Random.Range(-50.0f, 50.0f), 0, Random.Range(-5.0f, 5.0f));
            while(!InShootRange(newPosition) && !InAttackRange(newPosition))
            {
                newPosition = transform.position +
                              new Vector3(Random.Range(-50.0f, 50.0f), 0, Random.Range(-5.0f, 5.0f));
            }

            navMeshAgent.destination = newPosition;
        }
    }
}
