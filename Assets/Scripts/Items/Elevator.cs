using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : KillTech
{
    [Header("Elevator Settings")]
    [SerializeField] private Vector3 movement = new Vector3(0f, 30f, 0f);
    [SerializeField] [Range(0f, 10f)] private float moveSpeed = 5f;
    [SerializeField] [Range(0f, 5f)] private float wait = 2.5f;
    [SerializeField] private bool onlyMoveWhenCarrying = true;

    private bool active;
    private Vector3 origin;
    private bool going;
    private float currentWait;
    private Vector3 direction;
    private bool arrived;

    protected override void Activate() { active = true; }

    protected override void OnEnter() { player.transform.SetParent(transform);}
    protected override void OnExit() { player.transform.SetParent(null); }

    protected override void OnStart()
    {
        active = !killActivated;
        origin = transform.position;
        going = false;
        currentWait = -1f;
        direction = movement.normalized;
    }

    private void Update()
    {
        CheckArrival();
        if (active)
        {
            if (currentWait <= 0f)
            {
                if (!onlyMoveWhenCarrying) Move();
                else if (playerInRange || !arrived || going) Move();
            }
            else currentWait -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (arrived)
        {
            if (currentWait == -1f) currentWait = 0f;
            else currentWait = wait;
            going = !going;
        }
        else transform.position += direction * moveSpeed * Time.deltaTime * (going ? 1 : -1);
    }

    private void CheckArrival() { arrived = Vector3.Distance(transform.position, origin + (going ? movement : Vector3.zero)) <= moveSpeed * Time.deltaTime; }

    protected override void OnGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position+movement);
    }
}
