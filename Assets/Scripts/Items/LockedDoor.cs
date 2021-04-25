using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LockedDoor : MonoBehaviour
{
    [SerializeField] private DoorLock lockType = DoorLock.Key;

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

    private void Start()
    {
        if (sceneKey != null) code = sceneKey.GetComponent<KeyItem>().GetCode();
        else code = alternateCode;
        playerInRange = false;
        player = PlayerStats.Instance();
        kills = 0;
        EnemyPool();
    }

    private void OnDrawGizmos()  { if (lockType == DoorLock.Kills) { Gizmos.color = Color.red; Gizmos.DrawWireCube(transform.position + areaPosition, areaSize); } }

    private void Update()
    {
        if (lockType == DoorLock.Key)
        {
            if (playerInRange)
            {
                if (Input.GetButton("Fire2"))
                {
                    if (player.GetKey(code)) Open();
                    else Debug.Log("You don't have a " + code);
                }
            }
        }
    }

    private void Open()
    {
        //todo I guess just fucking open
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) { if (other.gameObject.CompareTag("Player")) playerInRange = true; }
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