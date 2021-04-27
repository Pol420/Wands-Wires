using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStateMachine), typeof(NavMeshAgent))]
public abstract class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] protected GameObject[] patrolPositions;
    [SerializeField] protected GameObject shootingPoint;
    protected int patrolPosition;
    protected GameObject player;
    protected Enemy_Data enemyData;
    protected float errorDistance = 2.0f;
    protected float timeToAttack;
    protected float timeToShoot;
    protected Animator animator;

    protected NavMeshAgent navMeshAgent;
    
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyData = GetComponent<Enemy_Data>();
        timeToAttack = 0.0f;
        timeToShoot = 0.0f;
        animator = GetComponent<Animator>();

        if (patrolPositions.Length <= 0)
            patrolPositions = DefaultPatrolPositions();
    }

    public void Chase()
    {
        navMeshAgent.destination = player.transform.position;
    }

    public void EndChase()
    {
        navMeshAgent.destination = transform.position;
    }

    public void Patrol()
    {
        if (CalculateDistance(patrolPositions[patrolPosition].transform.position, transform.position) < errorDistance)
        {
            patrolPosition++;
            if (patrolPosition >= patrolPositions.Length)
                patrolPosition = 0;
        }
            
        navMeshAgent.destination = patrolPositions[patrolPosition].transform.position;
    }

    public abstract void Attack();

    protected void LookAtPlayer()
    {
        var playerPosition = player.transform.position;
        transform.LookAt(new Vector3(playerPosition.x, transform.position.y, playerPosition.z));
    }

    public bool InAttackRange()
    {
        return CalculateDistance(player.transform.position, transform.position) <= enemyData.GetAttackDistance();
    }
    
    public bool InAttackRange(Vector3 position)
    {
        return CalculateDistance(player.transform.position, position) <= enemyData.GetAttackDistance();
    }

    public bool InShootRange()
    {
        return CalculateDistance(player.transform.position, transform.position) <= enemyData.GetShootDistance();
    }
    
    public bool InShootRange(Vector3 position)
    {
        return CalculateDistance(player.transform.position, position) <= enemyData.GetShootDistance();
    }
    
    public float CalculateDistance(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;

        return Vector3.Distance(a, b);
    }

    private GameObject[] DefaultPatrolPositions()
    {
        GameObject[] newPatrolPositions = GameObject.FindGameObjectsWithTag("PatrolPosition");

        if (newPatrolPositions.Length > 0)
            return newPatrolPositions;

        newPatrolPositions[0] = gameObject;
        return newPatrolPositions;
    }
}
