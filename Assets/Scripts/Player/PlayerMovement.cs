using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControls), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maximumVelocity = 64f;
    [SerializeField] private Feet feet = null;
    private PlayerControls controls;
    private Transform cam;
    protected Rigidbody body;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();
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
            if (body.velocity.y >= 0f)
            {
                body.velocity += Time.unscaledDeltaTime * Physics.gravity * body.mass * (controls.IsJumping() ? 0.5f : 1f);
                Move(controls.axis);
            }
            else
            {
                Move(controls.axis);
                body.velocity += Time.unscaledDeltaTime * Physics.gravity * body.mass;
            }
        }
        Debug.Log(Mathf.RoundToInt(body.velocity.magnitude));
        SpeedLimit();
    }

    private void Pivot(float angle)
    {
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
    private bool IsGrounded() { return feet.IsGrounded(); }
    private void Jump(Vector2 input)
    {
        body.AddForce(((transform.forward * input.y + transform.right * input.x) * body.mass + new Vector3(0f, controls.jumpPower * body.mass)) * Time.unscaledDeltaTime, ForceMode.Impulse);
    }

    private void SpeedLimit()
    {
        if (body.velocity.magnitude > maximumVelocity && body.velocity.y > 0f) body.velocity = body.velocity.normalized * maximumVelocity;
    }

}
