using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControls), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private Rigidbody body;
    private Transform cam;

    void Start()
    {
        controls = GetComponent<PlayerControls>();
        body = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }
    
    void Update()
    {
        Pivot(controls.mouse.x);
        Look(controls.mouse.y);
    }

    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            Move(controls.axis * (controls.Sprinting() ? 2f : 1f));
            if (controls.IsJumping()) Jump();
        }
        else Move(controls.axis * (controls.Sprinting() ? 1f : 0.5f));
    }

    private void Pivot(float angle)
    {
        Quaternion rotation = Quaternion.Euler(0f, angle * 90f * Time.fixedDeltaTime, 0f);
        body.MoveRotation(body.rotation * rotation);
    }
    private void Look(float angle)
    {
        cam.Rotate(Vector3.right, -angle * 90f * Time.deltaTime);
        if (cam.localEulerAngles.x < 180f)
        {
            if (cam.localEulerAngles.x > 89f) cam.localEulerAngles = new Vector3(89f, 0f, 0f);
        }
        else if (cam.localEulerAngles.x < 360f - 89f) cam.localEulerAngles = new Vector3(360f - 89f, 0f, 0f);
    }
    private void Move(Vector2 input)
    {
        transform.position += transform.forward * input.y * Time.deltaTime + transform.right * input.x * Time.deltaTime;
    }
    private bool IsGrounded()
    {
        if (Physics.Raycast(transform.position+Vector3.down, Vector3.down, out RaycastHit hit, Time.deltaTime * -Physics2D.gravity.y)) if (hit.collider.gameObject.CompareTag("Ground")) return true;
        return false;
    }
    private void Jump()
    {
        body.AddForce(new Vector3(0f, controls.jumpPower), ForceMode.Impulse);
    }
}
