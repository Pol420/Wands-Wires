using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDoor : MonoBehaviour
{
    [SerializeField] private GameObject leftKeySlot = null;
    [SerializeField] private GameObject rightKeySlot = null;
    [Header("Key Settings")]
    [SerializeField] private string leftKeyCode = "key1";
    [SerializeField] private string rightKeyCode = "key2";

    protected bool playerInRange;
    protected PlayerStats player;
    private bool leftOn;
    private bool rightOn;

    private void Start()
    {
        playerInRange = false;
        leftOn = false;
        rightOn = false;
        player = PlayerStats.Instance();
        leftKeySlot.SetActive(false);
        rightKeySlot.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                if (!leftOn)
                {
                    if (player.GetKey(leftKeyCode))
                    {
                        leftOn = true;
                        leftKeySlot.SetActive(true);
                    }
                    else Debug.Log("You don't have the left key!");
                }
                if (!rightOn)
                {
                    if (player.GetKey(rightKeyCode))
                    {
                        rightOn = true;
                        rightKeySlot.SetActive(true);
                    }
                    else Debug.Log("You don't have the right key!");
                }
                else if (leftOn)
                {
                    LevelManager.Instance().LoadNextScene();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) { if (other.gameObject.CompareTag("Player")) playerInRange = true; }
    private void OnTriggerExit(Collider other) { if (other.gameObject.CompareTag("Player")) playerInRange = false; }
}
