using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourFat : EnemyBehaviour
{
    public override void Attack()
    {
        LookAtPlayer();

        if (timeToAttack <= 0)
        {
            animator.SetTrigger("Attack");
            timeToAttack = enemyData.GetMaxTimeToAttack();
        }
        else timeToAttack -= Time.deltaTime;
    }
}
