using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Behaviour_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Enemy_Stats enemyStats;
    [SerializeField] private GameObject shootingPoint;

    [SerializeField] private GameObject[] patrolPositions;
    private int patrolPosition = 0;

    [SerializeField] private GameObject enemyToSpawn;

    private float timeToAttack;
    private float timeToShoot;

    private NavMeshAgent navMeshAgent;

    enum EnemyStates {ATTACK, CHASE, PATROL}
    private EnemyStates state;

    // Start is called before the first frame update
    void Start()
    {
        timeToAttack = enemyStats.maxTimeToAttack;
        timeToShoot = 0.0f;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = enemyStats.speed;
        state = EnemyStates.PATROL;
    }

    // Update is called once per frame
    void Update()
    {
        if (InDetectionRange())
        {
            if (InAttackRange())
            {
                EndChase();
                state = EnemyStates.ATTACK;
            }
            else
            {
                state = EnemyStates.CHASE;
            }
        }
        else
        {
            state = EnemyStates.PATROL;
        }
        
        switch(state)
        {
            case EnemyStates.CHASE:
                Chase();
                break;
            case EnemyStates.ATTACK:
                Attack();
                break;
            case EnemyStates.PATROL:
                Patrol();
                break;
            default:
                Idle();
                break;
        }
    }

    private void Chase()
    {
        navMeshAgent.destination = player.transform.position;
    }

    private void EndChase()
    {
        navMeshAgent.destination = transform.position;
    }

    private void Attack()
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
            timeToAttack = enemyStats.maxTimeToAttack;
        }
        else
        {
            timeToAttack -= Time.deltaTime;
        }
    }

    private void AvoidPlayer()
    {
        if (CalculateDistance(navMeshAgent.destination, transform.position) < 2.0f)
        {
            Vector3 newPosition = transform.position + transform.forward * - 
                (enemyStats.attackDistance - CalculateDistance(player.transform.position, transform.position));

            if (!ValidPosition(newPosition))
            {
                newPosition = transform.position + transform.forward *  
                    (enemyStats.attackDistance + CalculateDistance(player.transform.position, transform.position));

                if (!ValidPosition(newPosition))
                {
                    newPosition = transform.position + transform.right *  
                        (enemyStats.attackDistance - 2.0f);
                    if (!ValidPosition(newPosition))
                    {
                        newPosition = transform.position + transform.right *  
                            - (enemyStats.attackDistance - 2.0f);
                        
                    }
    
                }
            }

            navMeshAgent.destination = newPosition;
        }
    }

    private void Patrol()
    {
        if (Vector3.Distance(patrolPositions[patrolPosition].transform.position, transform.position) < 2.0f)
        {
            patrolPosition++;
            if (patrolPosition >= patrolPositions.Length)
                patrolPosition = 0;
        }
            
        navMeshAgent.destination = patrolPositions[patrolPosition].transform.position;
    }

    private void Idle()
    {
        
    }

    private bool InDetectionRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) <= enemyStats.maxSightDistance;
    }

    private bool InAttackRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) <= enemyStats.attackDistance;
    }
    
    private bool InAttackRange(Vector3 position)
    {
        return Vector3.Distance(player.transform.position, position) <= enemyStats.attackDistance;
    }

    private bool InShootRange(Vector3 position)
    {
        return Vector3.Distance(player.transform.position, position) <= enemyStats.shootDistance;
    }

    private bool InComfortZone(Vector3 position)
    {
        return CalculateDistance(player.transform.position, position) <= enemyStats.attackDistance && 
               CalculateDistance(player.transform.position, position) >= enemyStats.attackDistance - 10.0f;
    }

    private bool ValidPosition(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    private void LookAtPlayer()
    {
        var playerPosition = player.transform.position;
        transform.LookAt(new Vector3(playerPosition.x, transform.position.y, playerPosition.z));
        shootingPoint.transform.LookAt(player.transform.position + new Vector3(0,1,0));
    }

    private float CalculateDistance(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;

        return Vector3.Distance(a, b);
    }
}
