using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 mouseSensitivity = new Vector2(1f, 2f);
    [SerializeField] [Range(0f, 10f)] private float moveSpeed = 5f;
    [SerializeField] [Range(0f, 5f)] public float jumpPower = 5f;

    [Header("Current Values")]
    public Vector2 axis;
    public Vector2 mouse;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        axis = Vector2.zero;
        mouse = Vector2.zero;
    }
    
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        if(inputX * inputY == 0f) axis = new Vector2(inputX, inputY) * moveSpeed;
        else axis = new Vector2(inputX, inputY).normalized * moveSpeed;
        mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).normalized * mouseSensitivity;
    }

    public bool Sprinting() { return Input.GetKey(KeyCode.LeftShift); }
    public bool IsJumping() { return Input.GetKey(KeyCode.Space); }

}
