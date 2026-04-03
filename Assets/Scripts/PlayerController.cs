using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        if (movement != Vector2.zero)
            lastDirection = movement;

        UpdateAnimation();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    void UpdateAnimation()
    {
        float speed = movement.magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetFloat("MoveX", lastDirection.x);
        animator.SetFloat("MoveY", lastDirection.y);

        bool moving = speed > 0.1f;

        if (moving)
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                animator.Play("run_side");
                spriteRenderer.flipX = movement.x < 0;
                spriteRenderer.flipY = false;
            }
            else if (movement.y < 0)
            {
                animator.Play("run_down");
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }
            else
            {
                animator.Play("run_up");
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = true;
            }
        }
        else
        {
            if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
            {
                animator.Play("idle_side");
                spriteRenderer.flipX = lastDirection.x < 0;
                spriteRenderer.flipY = false;
            }
            else if (lastDirection.y < 0)
            {
                animator.Play("idle_down");
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }
            else
            {
                animator.Play("idle_up");
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }
        }
    }
}