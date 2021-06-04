using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : PickableObject
{
    [SerializeField] private string keyCode = "door1";

    protected override void OnPickup() {
        playerStats.AddKeyItem(keyCode, gameObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Otros/key", GetComponent<Transform>().position);
    }

    public string GetCode() { return keyCode; }

    protected override void OnStart() { }
}
