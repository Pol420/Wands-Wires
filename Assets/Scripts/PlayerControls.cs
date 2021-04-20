using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] [Range(0f, 20f)] private float moveSpeed = 10f;
    [SerializeField] [Range(0f, 20f)] public float jumpPower = 10f;

    [Header("Current Values")]
    public Vector2 axis;
    public Vector2 mouse;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
        axis = Vector2.zero;
        mouse = Vector2.zero;
    }
    
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        if(inputX * inputY == 0f) axis = new Vector2(inputX, inputY) * moveSpeed;
        else axis = new Vector2(inputX, inputY).normalized * moveSpeed;
        mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * Screen.currentResolution.height / Screen.currentResolution.width) * mouseSensitivity;
    }
    
    public bool IsJumping() { return Input.GetKey(KeyCode.Space); }

}
