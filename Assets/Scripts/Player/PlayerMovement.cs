using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControls))]
public class PlayerMovement : Pushable
{
    [SerializeField] private Transform feet = null;
    private PlayerControls controls;
    private Transform cam;

    void Start()
    {
        controls = GetComponent<PlayerControls>();
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
            Move(controls.axis);
            if (controls.IsJumping()) Jump(controls.axis);
        }
        else
        {
            body.velocity += Time.unscaledDeltaTime * Physics.gravity * body.mass / 2f;
            if (body.velocity.y >= 0f) Move(controls.axis);
            else
            {
                Move(controls.axis);
                body.velocity += Time.unscaledDeltaTime * Physics.gravity * body.mass / 2f;
            }
        }
    }

    private void Pivot(float angle)
    {
        //body.MoveRotation(body.rotation * Quaternion.Euler(0f, angle, 0f));
        transform.rotation = transform.rotation * Quaternion.Euler(0f, angle, 0f);
    }
    private void Look(float angle)
    {
        Vector3 euler = cam.eulerAngles;
        euler.x = Mathf.Clamp(((euler.x + 180f) % 360f) - 180f - angle, -90f, 90f);
        cam.rotation = Quaternion.Euler(euler);
    }
    private void Move(Vector2 input)
    {
        Vector3 movement = (transform.forward * input.y + transform.right * input.x) * Time.unscaledDeltaTime;
        movement.y = 0f;
        body.MovePosition(transform.position + movement);
    }
    private bool IsGrounded()
    {
        if (Physics.Raycast(feet.position, Vector3.down, out RaycastHit hit, Time.unscaledDeltaTime * -Physics2D.gravity.y * 2f))
            if (hit.collider.gameObject.CompareTag("Ground") && Vector3.Angle(Vector3.up, hit.normal) <= 60f) return true;
        return false;
    }
    private void Jump(Vector2 input)
    {
        body.AddForce(((transform.forward * input.y + transform.right * input.x) + new Vector3(0f, controls.jumpPower * body.mass)) * Time.unscaledDeltaTime, ForceMode.Impulse);
    }
}
