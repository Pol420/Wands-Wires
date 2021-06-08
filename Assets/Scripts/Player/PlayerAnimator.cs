using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] private float bobbingMultiplier = 1f;
    private static Animator anim;

    private static float counter;
    private Vector3 previousPosition;
    private Transform body;

    void Awake()
    {
        anim = GetComponent<Animator>();
        counter = 0f;
        body = transform.GetChild(0);
        previousPosition = body.position;
    }

    private void Update()
    {
        Vector3 velocity = body.position - previousPosition;
        velocity.y = 0f;
        anim.SetFloat("speed", velocity.magnitude);
        previousPosition = body.position;
    }

    public static void Reload()
    {
        anim.SetTrigger("Reload");
    }

}
