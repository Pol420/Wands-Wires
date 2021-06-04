using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(EnemyBehaviour), typeof(Enemy_Data))]
public class EnemyStateMachine : MonoBehaviour
{
    private Transform player;
    private Enemy_Data enemyData;
    private EnemyBehaviour enemyBehaviour;

    enum EnemyStates { Attack, Chase, Patrol }
    private EnemyStates state;
    
    void Start()
    {
        state = EnemyStates.Patrol;
        player = PlayerStats.Instance().transform.GetChild(0);
        enemyData = GetComponent<Enemy_Data>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    void Update()
    {
        if (!LevelManager.paused)
        {
            ChangeState();
            switch (state)
            {
                case EnemyStates.Chase: Chase(); break;
                case EnemyStates.Attack: Attack(); break;
                default: Patrol(); break;
            }
        }
    }

    private void ChangeState()
    {
        if (InDetectionRange() && CanSeePlayer())
        {
            if (InAttackRange() || InShootDistance())
            {
                EndChase();
                state = EnemyStates.Attack;
            }
            else state = EnemyStates.Chase;
        }
        else state = EnemyStates.Patrol;
    }
    private bool CanSeePlayer()
    {
        Vector3 dir = player.position - transform.position;
        Debug.DrawRay(transform.position, dir, Color.red);
        if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, dir.magnitude))
        {
            if (hit.collider.gameObject.CompareTag("Player")) return true;
        }
        return false;

        
    }

    private void Chase() { enemyBehaviour.Chase(); }
    private void EndChase() { enemyBehaviour.EndChase(); }
    private void Attack() { enemyBehaviour.Attack(); }
    private void Patrol() { enemyBehaviour.Patrol(); }

    private bool InDetectionRange()
    {
        return enemyBehaviour.CalculateDistance(player.position, transform.position) <= enemyData.GetMaxSightDistance();
    }
    private bool InAttackRange()
    {
        return enemyBehaviour.CalculateDistance(player.position, transform.position) <= enemyData.GetAttackDistance();
    }
    private bool InShootDistance()
    {
        return enemyBehaviour.CalculateDistance(player.position, transform.position) <= enemyData.GetShootDistance();
    }
}
