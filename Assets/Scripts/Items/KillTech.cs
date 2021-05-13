using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KillTech : MonoBehaviour
{
    [SerializeField] protected bool killActivated = false;
    [Header("Kills Settings")]
    [SerializeField] private int killsToActivate = 20;
    [SerializeField] private bool killAll = false;
    [SerializeField] private Vector3 areaPosition = Vector3.zero;
    [SerializeField] private Vector3 areaSize = Vector3.one;
    private int kills;

    protected bool playerInRange;
    protected PlayerStats player;

    private void Start()
    {
        playerInRange = false;
        player = PlayerStats.Instance();
        kills = 0;
        EnemyPool();
        OnStart();
    }

    private void OnDrawGizmos() { OnGizmos(); if (killActivated) { Gizmos.color = Color.red; Gizmos.DrawWireCube(transform.position + areaPosition, areaSize); } }
    

    protected abstract void Activate();
    protected abstract void OnStart();
    protected abstract void OnEnter();
    protected abstract void OnExit();
    protected abstract void OnGizmos();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            OnEnter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            OnExit();
        }
    }

    protected void EnemyPool()
    {
        if (killAll) kills = 0;
        Collider[] colliders = Physics.OverlapBox(transform.position + areaPosition, areaSize / 2f);
        int enemyCount = 0;
        foreach (Collider c in colliders)
        {
            Enemy enemy = c.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.singularDeath.AddListener(AddKill);
                enemyCount++;
            }
        }
        killsToActivate = Mathf.Min(killsToActivate, enemyCount);
    }

    private void AddKill()
    {
        if (killActivated)
        {
            if (killAll) EnemyPool();
            else kills++;
            if (kills >= killsToActivate) Activate();
        }
    }
}
