using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BoundingBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Feet>() == null)
            other.transform.position = Vector3.up+other.transform.localScale;
    }
}
