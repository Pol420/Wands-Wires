using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControls), typeof(Rigidbody), typeof(Collider))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maximumVelocity = 64f;
    [SerializeField] private LayerMask groundMask = 0;
    private PlayerControls controls;
    private Transform cam;
    protected Rigidbody body;
    private float fixedTimeFlow;


    private bool grounded;
    private float offFeet;
    private Vector3 feetSize;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();
        controls = GetComponent<PlayerControls>();
        cam = Camera.main.transform;
        grounded = false;
        offFeet = GetComponent<Collider>().bounds.size.y / 2f;
        feetSize = new Vector3(0.25f, 0.1f, 0.25f);
    }
    
    void Update()
    {
        if (!LevelManager.paused)
        {
            body.useGravity = true;
            Pivot(controls.mouse.x);
            Look(controls.mouse.y);
        }
        else body.useGravity = false;
        
        //if(Physics.Raycast(cam.position, cam.forward, out RaycastHit hit))
          //  Debug.Log(hit.transform.gameObject.name);
    }

    private void FixedUpdate()
    {
        if (!LevelManager.paused)
        {
            fixedTimeFlow = Time.fixedDeltaTime / Time.timeScale;
            if (grounded)
            {
                Move(controls.axis);
                if (controls.IsJumping()) Jump(controls.axis);
            }
            else
            {
                if (body.velocity.y >= 0f)
                {
                    Move(controls.axis);
                    Fall(fixedTimeFlow * (controls.IsJumping() ? 0.5f : 1f));
                }
                else
                {
                    Move(controls.axis);
                    Fall(fixedTimeFlow);
                }
            }
            SpeedLimit();
            grounded = Physics.CheckBox(transform.position - transform.up * offFeet, feetSize, transform.rotation, groundMask);
        }
    }

    private void Fall(float multiplier)
    {
        body.velocity += multiplier * Physics.gravity * body.mass;
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
        Vector3 movement = (transform.forward * input.y + transform.right * input.x) * fixedTimeFlow;
        movement.y = 0f;
        body.MovePosition(transform.position + movement);
    }
    private void Jump(Vector2 input)
    {
        body.AddForce(((transform.forward * input.y + transform.right * input.x) * body.mass + new Vector3(0f, controls.jumpPower * body.mass)) * fixedTimeFlow, ForceMode.Impulse);
    }

    private void SpeedLimit()
    {
        if (body.velocity.magnitude > maximumVelocity / Time.timeScale && body.velocity.y > 0f) body.velocity = body.velocity.normalized * maximumVelocity;
    }

}
