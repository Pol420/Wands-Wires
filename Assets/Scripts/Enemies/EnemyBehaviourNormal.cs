using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourNormal : EnemyBehaviour
{
    [SerializeField] private GameObject attackArea;
    
    public override void Attack()
    {
        LookAtPlayer();
        if (!enemy.IsDamaged())
        {
            if (InAttackRange())
            {
                MeleeAttack();  
            }
            else
            {
                ShootingAttack();
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
                    animator.SetBool("Walking", true);
                }
            }    
        }
    }

    private void MeleeAttack()
    {
        if (timeToAttack <= 0)
        {
            if (!enemy.IsDead())
            {
                animator.SetTrigger("Attack");
                FMODUnity.RuntimeManager.PlayOneShot("event:/EnemyNormal/hit", GetComponent<Transform>().position);
            }
                
            timeToAttack = enemyData.GetMaxTimeToAttack();
        }
        else
        {
            timeToAttack -= Time.deltaTime;
        } 
    }

    private void ShootingAttack()
    {
        if (timeToShoot <= 0 && !enemy.IsDead())
        {
            Instantiate(enemyData.GetProjectile(), shootingPoint.transform.position, shootingPoint.transform.rotation);
            timeToShoot = enemyData.GetMaxTimeToShoot();
            animator.SetTrigger("Shoot");
            FMODUnity.RuntimeManager.PlayOneShot("event:/EnemyNormal/shoot", GetComponent<Transform>().position);
        }
        else
        {
            timeToShoot -= Time.deltaTime;
        }
    }

    public void ActivateAttackArea()
    {
        attackArea.SetActive(true);
    }
    
    public void DeactivateAttackArea()
    {
        attackArea.SetActive(false);
    }

    void PlaySound(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, GetComponent<Transform>().position);
    }
}
