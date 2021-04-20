using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Vector3 = UnityEngine.Vector3;

public class Enemy_Behaviour_Fat : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Enemy_Stats enemyStats;
    [SerializeField] private GameObject shootingPoint;

    [SerializeField] private GameObject[] patrolPositions;
    private int patrolPosition = 0;

    private float timeToAttack;

    private NavMeshAgent navMeshAgent;

    enum EnemyStates {ATTACK, CHASE, PATROL}
    private EnemyStates state;

    // Start is called before the first frame update
    void Start()
    {
        timeToAttack = 0.0f;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = enemyStats.speed;
        state = EnemyStates.PATROL;
    }

    // Update is called once per frame
    void Update()
    {
        if (InDetectionRange())
        {
            state = InAttackRange() ? EnemyStates.ATTACK : EnemyStates.CHASE;
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
        EndChase();

        LookAtPlayer();
        
        if (timeToAttack <= 0)
        {
            Instantiate(enemyStats.projectile, shootingPoint.transform.position, shootingPoint.transform.rotation);
            timeToAttack = enemyStats.maxTimeToAttack;
        }
        else
        {
            timeToAttack -= Time.deltaTime;
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
    
    private void LookAtPlayer()
    {
        var playerPosition = player.transform.position;
        transform.LookAt(new Vector3(playerPosition.x, transform.position.y, playerPosition.z));
    }
}
