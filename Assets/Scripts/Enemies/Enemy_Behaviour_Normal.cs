using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Behaviour_Normal : MonoBehaviour
{
    [SerializeField] private Enemy_Stats enemyStats;
    [SerializeField] private GameObject shootingPoint;

    [SerializeField] private GameObject[] patrolPositions;
    private int patrolPosition = 0;

    private float timeToAttack;
    private float timeToShoot;

    private NavMeshAgent navMeshAgent;

    private Animator animator;
    private Transform player;

    enum EnemyStates {ATTACK, CHASE, PATROL}
    private EnemyStates state;

    // Start is called before the first frame update
    void Start()
    {
        timeToAttack = 0.0f;
        timeToShoot = 0.0f;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = enemyStats.speed;
        state = EnemyStates.PATROL;
        animator = GetComponent<Animator>();
        player = PlayerStats.Instance().transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (InDetectionRange())
        {
            if (InAttackRange() || InShootRange())
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
        navMeshAgent.destination = player.position;
    }

    private void EndChase()
    {
        navMeshAgent.destination = transform.position;
    }

    private void Attack()
    {
        LookAtPlayer();

        if (InAttackRange())
        {
            MeleeAttack();  
        }
        else
        {
            ShootingAttack();
            if (Vector3.Distance(navMeshAgent.destination, transform.position) < 2.0f)
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

    private void MeleeAttack()
    {
        if (timeToAttack <= 0)
        {
            animator.SetTrigger("Attack");
            timeToAttack = enemyStats.maxTimeToAttack;
        }
        else
        {
            timeToAttack -= Time.deltaTime;
        } 
    }

    private void ShootingAttack()
    {
        if (timeToShoot <= 0)
        {
            Instantiate(enemyStats.projectile, shootingPoint.transform.position, shootingPoint.transform.rotation);
            timeToShoot = enemyStats.maxTimeToShoot;
        }
        else
        {
            timeToShoot -= Time.deltaTime;
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
        return Vector3.Distance(player.position, transform.position) <= enemyStats.maxSightDistance;
    }

    private bool InAttackRange()
    {
        return Vector3.Distance(player.position, transform.position) <= enemyStats.attackDistance;
    }
    
    private bool InAttackRange(Vector3 position)
    {
        return Vector3.Distance(player.position, position) <= enemyStats.attackDistance;
    }

    private bool InShootRange()
    {
        return Vector3.Distance(player.position, transform.position) <= enemyStats.shootDistance;
    }
    
    private bool InShootRange(Vector3 position)
    {
        return Vector3.Distance(player.position, position) <= enemyStats.shootDistance;
    }

    private void LookAtPlayer()
    {
        var playerPosition = player.position;
        transform.LookAt(new Vector3(playerPosition.x, transform.position.y, playerPosition.z));
        shootingPoint.transform.LookAt(player.position + new Vector3(0,1,0));
    }
}
