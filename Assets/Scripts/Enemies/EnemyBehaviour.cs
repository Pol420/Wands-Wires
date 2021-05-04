using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent), typeof(Enemy_Data))]
public abstract class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] protected GameObject[] patrolPositions;
    [SerializeField] protected Transform shootingPoint = null;
    protected int patrolPosition;
    protected Transform player;
    protected Enemy_Data enemyData;
    protected float errorDistance = 2.0f;
    protected float timeToAttack;
    protected float timeToShoot;
    protected Animator animator;

    protected NavMeshAgent navMeshAgent;
    
    void Start()
    {
        player = PlayerStats.Instance().transform.GetChild(0);
        enemyData = GetComponent<Enemy_Data>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if(shootingPoint == null) shootingPoint = transform.GetChild(0);
        navMeshAgent.speed = enemyData.GetSpeed();
        timeToAttack = 0.0f;
        timeToShoot = 0.0f;
        patrolPosition = 0;

        if (patrolPositions.Length <= 0)
            patrolPositions = DefaultPatrolPositions();
    }

    public void Chase()
    {
        navMeshAgent.destination = player.position;
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
    
    protected virtual void AvoidPlayer()
    {
        if (CalculateDistance(navMeshAgent.destination, transform.position) < errorDistance)
        {
            Vector3 newPosition = transform.position + transform.forward * - 
                (enemyData.GetShootDistance() - CalculateDistance(player.position, transform.position));

            if (!ValidPosition(newPosition))
            {
                newPosition = transform.position + transform.forward *  
                    (enemyData.GetShootDistance() + CalculateDistance(player.position, transform.position));

                if (!ValidPosition(newPosition))
                {
                    newPosition = transform.position + transform.right *  
                        (enemyData.GetShootDistance() - errorDistance);
                    if (!ValidPosition(newPosition))
                    {
                        newPosition = transform.position + transform.right *  
                            - (enemyData.GetShootDistance() - errorDistance);
                    }
                }
            }
            navMeshAgent.destination = newPosition;
        }
    }

    protected void LookAtPlayer()
    {
        var playerPosition = player.position;
        transform.LookAt(new Vector3(playerPosition.x, transform.position.y, playerPosition.z));
        if(shootingPoint != null) shootingPoint.LookAt(playerPosition + new Vector3(0,1,0));
    }

    public bool InAttackRange()
    {
        return CalculateDistance(player.position, transform.position) <= enemyData.GetAttackDistance();
    }
    
    public bool InAttackRange(Vector3 position)
    {
        return CalculateDistance(player.position, position) <= enemyData.GetAttackDistance();
    }

    public bool InShootRange()
    {
        return CalculateDistance(player.position, transform.position) <= enemyData.GetShootDistance();
    }
    
    protected bool InShootRange(Vector3 position)
    {
        return CalculateDistance(player.position, position) <= enemyData.GetShootDistance();
    }
    
    protected virtual bool InComfortZone(Vector3 position)
    {
        return CalculateDistance(player.position, position) <= enemyData.GetShootDistance() && 
               CalculateDistance(player.position, position) >= enemyData.GetShootDistance() - 10.0f;
    }

    protected bool ValidPosition(Vector3 position)
    {
        var path = new NavMeshPath();
        navMeshAgent.CalculatePath(position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }
    
    public float CalculateDistance(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;
        return Vector3.Distance(a, b);
    }

    private GameObject[] DefaultPatrolPositions()
    {
        var newPatrolPositions = GameObject.FindGameObjectsWithTag("PatrolPosition");
        
        if (newPatrolPositions.Length > 0)
            return newPatrolPositions;

        newPatrolPositions = new GameObject[1];
        newPatrolPositions[0] = gameObject;
        return newPatrolPositions;
    }
}
