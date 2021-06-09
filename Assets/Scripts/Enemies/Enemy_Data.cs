using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Data : MonoBehaviour
{
    [SerializeField] private Enemy_Stats enemyStats;
    
    public int GetMaxHealth() { return enemyStats.maxHealth; }
    public float GetAttackDamage() { return enemyStats.attackDamage; }
    public float GetMaxSightDistance() { return enemyStats.maxSightDistance; }
    public float GetAttackDistance() { return enemyStats.attackDistance; }
    public float GetShootDistance() { return enemyStats.shootDistance; }
    public float GetMaxTimeToAttack() { return enemyStats.maxTimeToAttack; }
    public float GetMaxTimeToShoot() { return enemyStats.maxTimeToShoot; }
    public float GetSpeed() { return enemyStats.speed; }
    public GameObject GetProjectile() { return enemyStats.projectile; }
}
