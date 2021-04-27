using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private GameObject player;
    private Enemy_Data enemyData;
    private EnemyBehaviour enemyBehaviour;

    enum EnemyStates { Attack, Chase, Patrol }
    private EnemyStates state;
    
    void Start()
    {
        state = EnemyStates.Patrol;
        player = GameObject.FindGameObjectWithTag("Player");
        enemyData = GetComponent<Enemy_Data>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    
    void Update()
    {
        ChangeState();

        switch (state)
        {
            case EnemyStates.Chase:
                Chase();
                break;
            case EnemyStates.Attack:
                Attack();
                break;
            default:
                Patrol();
                break;
        }
    }

    private void ChangeState()
    {
        if (InDetectionRange())
        {
            if (InAttackRange() || InShootDistance())
            {
                EndChase();
                state = EnemyStates.Attack;
            }
            else
            {
                state = EnemyStates.Chase;
            }
        }
        else
        {
            state = EnemyStates.Patrol;
        }
    }

    private void Chase()
    {
        enemyBehaviour.Chase();
    }

    private void EndChase()
    {
        enemyBehaviour.EndChase();
    }

    private void Attack()
    {
        enemyBehaviour.Attack();
    }

    private void Patrol()
    {
        enemyBehaviour.Patrol();
    }

    private bool InDetectionRange()
    {
        return enemyBehaviour.CalculateDistance(player.transform.position, transform.position) <= enemyData.GetMaxSightDistance();
    }

    private bool InAttackRange()
    {
        return enemyBehaviour.CalculateDistance(player.transform.position, transform.position) <= enemyData.GetAttackDistance();
    }
    
    private bool InShootDistance()
    {
        return enemyBehaviour.CalculateDistance(player.transform.position, transform.position) <= enemyData.GetShootDistance();
    }
}
