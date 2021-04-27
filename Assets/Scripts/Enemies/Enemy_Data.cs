using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Data : MonoBehaviour
{
    [SerializeField] private Enemy_Stats enemyStats;

    private void Awake()
    {

    }

    public float GetAttackDamage()
    {
        return enemyStats.attackDamage;
    }
}
