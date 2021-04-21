using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyDoor : MonoBehaviour
{
    [SerializeField] private GameObject sceneKey = null;
    [SerializeField] private string alternateCode = "door1";
    private string code;
    private bool playerInRange;
    private PlayerStats player;

    private void Start()
    {
        if (sceneKey != null) code = sceneKey.GetComponent<KeyItem>().GetCode();
        else code = alternateCode;
        playerInRange = false;
        player = PlayerStats.Instance();
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (Input.GetButton("Fire2"))
            {
                if (player.GetKey(code)) Open();
                else Debug.Log("You don't have a " + code);
            } 
        }
    }

    private void Open()
    {
        Debug.Log("Door opened succesfully with "+code);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) { if (other.gameObject.CompareTag("Player")) playerInRange = true; }
    private void OnTriggerExit(Collider other) { if (other.gameObject.CompareTag("Player")) playerInRange = false; }
}
