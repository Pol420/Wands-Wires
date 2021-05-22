using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] [Range(0f, 32f)] private float moveSpeed = 16f;
    [SerializeField] [Range(0f, 2048f)] public float jumpPower = 1024f;

    [Header("Current Values")]
    public Vector2 axis;
    public Vector2 mouse;
    
    void Start()
    {
        axis = Vector2.zero;
        mouse = Vector2.zero;
    }

    void Update()
    {
        if (!LevelManager.paused)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            if (inputX * inputY == 0f) axis = new Vector2(inputX, inputY) * moveSpeed;
            else axis = new Vector2(inputX, inputY).normalized * moveSpeed;
            mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity;
        }
    }
    
    public bool IsJumping() { return Input.GetKey(KeyCode.Space); }
}
