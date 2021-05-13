using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class LockedDoor : KillTech
{
    [SerializeField] private bool trapDoor = false;

    [Header("Key Settings")]
    [SerializeField] private GameObject sceneKey = null;
    [SerializeField] private string alternateCode = "door1";
    private string code;
    
    private bool trapped;
    private List<Collider> colliders;
    private MeshRenderer doorRenderer;

    protected override void OnStart()
    {
        colliders = new List<Collider>();
        doorRenderer = GetComponent<MeshRenderer>();
        foreach (Collider c in GetComponentsInChildren<Collider>()) if (!c.isTrigger) colliders.Add(c);
        if (sceneKey != null) code = sceneKey.GetComponent<KeyItem>().GetCode();
        else code = alternateCode;
        if (trapDoor) ActivateCollisions(false);
        trapped = trapDoor;
    }
    private void ActivateCollisions(bool active)
    {
        doorRenderer.enabled = active;
        foreach (Collider c in colliders) c.enabled = active;
    }

    private void Update()
    {
        if (killActivated)
        {
            if (playerInRange)
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    if (player.GetKey(code)) Activate();
                    else Debug.Log("You don't have a " + code);
                }
            }
        }
    }
    protected override void Activate() { ActivateCollisions(false); }

    protected override void OnEnter()
    {
        if (trapped)
        {
            ActivateCollisions(true);
            trapped = false;
            EnemyPool();
        }
    }

    protected override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnGizmos() { }
}