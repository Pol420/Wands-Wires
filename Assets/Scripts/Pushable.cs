using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Pushable : MonoBehaviour
{
    protected Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Push(Vector3 direction, float amount)
    {
        body.AddForce(direction * amount, ForceMode.Force);
    }
    
}
