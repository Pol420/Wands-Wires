using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Feet : MonoBehaviour
{
    private List<GameObject> grounds;

    private void Start() { grounds = new List<GameObject>(); }

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Ground")) grounds.Add(other.gameObject); }
    private void OnTriggerExit(Collider other) { if (other.CompareTag("Ground")) grounds.Remove(other.gameObject); }

    public bool IsGrounded()
    {
        Debug.Log(grounds.Count);
        return grounds.Count > 0;
    }

}
