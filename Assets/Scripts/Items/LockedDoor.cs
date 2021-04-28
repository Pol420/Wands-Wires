using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class LockedDoor : MonoBehaviour
{
    [SerializeField] private DoorLock lockType = DoorLock.Key;
    [SerializeField] private bool trapDoor = false;

    [Header("Key Settings")]
    [SerializeField] private GameObject sceneKey = null;
    [SerializeField] private string alternateCode = "door1";
    private string code;

    [Header("Kills Settings")]
    [SerializeField] private int killsToOpen = 20;
    [SerializeField] private Vector3 areaPosition = Vector3.zero;
    [SerializeField] private Vector3 areaSize = Vector3.one;
    private int kills;

    private bool playerInRange;
    private PlayerStats player;
    private bool trapped;
    private List<Collider> colliders;
    private MeshRenderer doorRenderer;

    private void Start()
    {
        colliders = new List<Collider>();
        doorRenderer = GetComponent<MeshRenderer>();
        foreach (Collider c in GetComponentsInChildren<Collider>()) if (!c.isTrigger) colliders.Add(c);
        if (sceneKey != null) code = sceneKey.GetComponent<KeyItem>().GetCode();
        else code = alternateCode;
        playerInRange = false;
        player = PlayerStats.Instance();
        kills = 0;
        EnemyPool();
        if (trapDoor) ActivateCollisions(false);
        trapped = trapDoor;
    }

    private void ActivateCollisions(bool active)
    {
        doorRenderer.enabled = active;
        foreach (Collider c in colliders) c.enabled = active;
    }

    private void OnDrawGizmos()  { if (lockType == DoorLock.Kills) { Gizmos.color = Color.red; Gizmos.DrawWireCube(transform.position + areaPosition, areaSize); } }

    private void Update()
    {
        if (lockType == DoorLock.Key)
        {
            if (playerInRange)
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    if (player.GetKey(code)) Open();
                    else Debug.Log("You don't have a " + code);
                }
            }
        }
    }

    private void Open() { ActivateCollisions(false); }
    private void Close() { ActivateCollisions(true); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (trapped)
            {
                ActivateCollisions(true);
                trapped = false;
            }
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other) { if (other.gameObject.CompareTag("Player")) playerInRange = false; }

    private void EnemyPool()
    {
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
        killsToOpen = Mathf.Min(killsToOpen, enemyCount);
    }

    private void AddKill() { kills++; if (kills >= killsToOpen) Open(); }
}
public enum DoorLock { Key, Kills}