using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteController : MonoBehaviour
{
    public RuntimeAnimatorController animatorRight;
    public Sprite spriteRight;
    public RuntimeAnimatorController animatorLeft;
    public Sprite spriteLeft;
    public RuntimeAnimatorController animatorUp;
    public Sprite spriteUp;
    public RuntimeAnimatorController animatorDown;
    public Sprite spriteDown;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 movement = rb.velocity;

        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            if (movement.x > 0)
            {
                animator.runtimeAnimatorController = animatorRight;
                spriteRenderer.sprite = spriteRight;
            }
            else
            {
                animator.runtimeAnimatorController = animatorLeft;
                spriteRenderer.sprite = spriteLeft;
            }
        }
        else 
        {
            if (movement.y > 0)
            {
                animator.runtimeAnimatorController = animatorUp;
                spriteRenderer.sprite = spriteUp;
            }
            else
            {
                animator.runtimeAnimatorController = animatorDown;
                spriteRenderer.sprite = spriteDown;
            }
        }
    }
}

