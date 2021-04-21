using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class PickableObject : MonoBehaviour
{
    protected PlayerStats player;

    private void Start()
    {
        player = PlayerStats.Instance();
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) { if (other.gameObject.CompareTag("Player")) PickUp(); }
    private void PickUp()
    {
        OnPickup();
        Destroy(gameObject);
    }

    protected abstract void OnPickup();
}
