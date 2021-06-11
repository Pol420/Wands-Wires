using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] private float bobbingMultiplier = 1f;
    [SerializeField] [Range(0f, 1f)] private float pivotingAmount = 0.1f;
    private static Animator anim;

    private Vector3 previousPosition;
    private Transform body;
    private float direction;

    void Awake()
    {
        InitAnimator();
    }

    public void InitAnimator()
    {
        anim = GetComponent<Animator>();
        body = transform.GetChild(0);
        previousPosition = body.position;
    }

    private void Update()
    {
        if (!LevelManager.paused)
        {
            direction = Mathf.Lerp(direction, Input.GetAxis("Mouse X") * pivotingAmount, pivotingAmount);
            Vector3 velocity = body.position - previousPosition;
            velocity.y = 0f;
            anim.SetFloat("speed", velocity.magnitude * bobbingMultiplier - 1f);
            previousPosition = body.position;
            anim.SetFloat("direction", direction);
        }
    }

    public static void Reload()
    {
        anim.SetTrigger("Reload");
    }
}
