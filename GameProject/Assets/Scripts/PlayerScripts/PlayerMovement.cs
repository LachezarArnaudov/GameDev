using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Respawn System")]
    public Vector3 lastSafePosition;

    [Header("Unlocked Abilities")]
    public bool hasDash = false;
    public bool hasDoubleJump = false;

    [Header("Dash Settings")]
    public float dashSpeed = 24f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing;
    private bool canDashFlag = true;

    [Header("DoubleJump Settings")]
    public int extraJumpsValue = 1;
    private int jumpsLeft;

    [Header("Main Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Jump smoothness")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Coyote Time & Jump Buffer")]
    public float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    [Header("GroundCheck")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDashing) return;

        if (hasDash && Input.GetKeyDown(KeyCode.LeftShift) && canDashFlag)
        {
            StartCoroutine(PerformDash());
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        if (isGrounded)
        {
            jumpsLeft = extraJumpsValue;
            coyoteTimeCounter = coyoteTime;
            lastSafePosition = transform.position;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpBufferCounter = 0f;
            }
            else if (jumpsLeft > 0 && hasDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpsLeft--;
                jumpBufferCounter = 0f;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * lowJumpMultiplier);
            coyoteTimeCounter = 0f;
        }

        Flip();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void UpdateAnimations()
    {
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));

        anim.SetBool("IsGrounded", isGrounded);        
    }

    private System.Collections.IEnumerator PerformDash()
    {
        GetComponent<DashTrail>().StartTrail(dashDuration);

        canDashFlag = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(Mathf.Sign(transform.localScale.x) * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDashFlag = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}