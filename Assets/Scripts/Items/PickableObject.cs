using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public abstract class PickableObject : MonoBehaviour
{
    protected PlayerStats playerStats;
    protected PlayerPowers playerPowers;
    protected Rigidbody body;
    protected Collider col;


    private void Awake()
    {
        foreach (Collider c in GetComponents<Collider>()) if (c.isTrigger) col = c;
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerStats = PlayerStats.Instance();
        playerPowers = PlayerPowers.Instance();
        OnStart();
    }

    private void OnTriggerEnter(Collider other) { if (other.gameObject.CompareTag("Player")) PickUp(); }
    private void PickUp()
    {
        OnPickup();
        Destroy(gameObject);
    }

    protected abstract void OnPickup();
    protected abstract void OnStart();

    public void Activate(bool active)
    {
        foreach (Collider c in GetComponents<Collider>()) c.enabled = active;
        body.useGravity = active;
        if (!active) body.velocity = Vector3.zero;
        else body.AddForce(5f * (Vector3.up + Random.insideUnitSphere.normalized), ForceMode.Impulse);
    }
}
