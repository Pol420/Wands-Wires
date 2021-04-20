using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/Enemy_Stats", order = 1)]
public class Enemy_Stats : ScriptableObject
{
    [SerializeField] public float attackDistance;
    [SerializeField] public float shootDistance;
    [SerializeField] public float shootDamage;
    [SerializeField] public float attackDamage;
    [SerializeField] public float maxSightDistance;
    [SerializeField] public float maxTimeToShoot;
    [SerializeField] public float maxTimeToAttack;
    [SerializeField] public float maxHealth;
    [SerializeField] public float speed;
    [SerializeField] public GameObject projectile;

}
