using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourFat : EnemyBehaviour
{
    [SerializeField] private GameObject attackArea;
    
    public override void Attack()
    {
        LookAtPlayer();

        if (timeToAttack <= 0)
        {
            animator.SetTrigger("Attack");
            timeToAttack = enemyData.GetMaxTimeToAttack();
            FMODUnity.RuntimeManager.PlayOneShot("event:/EnemyNormal/hit", GetComponent<Transform>().position);
        }
        else timeToAttack -= Time.deltaTime;
    }
    
    public void ActivateAttackArea()
    {
        attackArea.SetActive(true);
    }
    
    public void DeactivateAttackArea()
    {
        attackArea.SetActive(false);
    }
}
