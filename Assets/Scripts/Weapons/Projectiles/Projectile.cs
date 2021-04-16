using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] public int damage = 10;
    [SerializeField] public Ammo type = Ammo.Fire;
    protected Rigidbody body;

    private void OnCollisionEnter(Collision collision) { OnCollision(collision); }
    private void OnCollisionStay(Collision collision) { OnDrag(collision); }

    protected abstract void OnCollision(Collision collision);
    protected abstract void OnDrag(Collision collision);

}
