using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : PickableObject
{
    [SerializeField] private string keyCode = "door1";

    protected override void OnPickup() {  player.AddKeyItem(keyCode, gameObject); }

    public string GetCode() { return keyCode; }
}
