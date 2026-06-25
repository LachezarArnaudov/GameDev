using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Respawn System")]
    public Vector3 lastSafePosition;

    [Header("Unlocked Abilities")]
    public bool hasDash = false;
    public bool hasDoubleJump = false;
    public bool hasWallJump = false;

    [Header("Dash Settings")]
    public float dashSpeed = 24f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing;
    private bool canDashFlag = true;

    [Header("DoubleJump Settings")]
    public int extraJumpsValue = 1;
    private int jumpsLeft;

    [Header("Wall Slide & Jump Settings")]
    public Transform wallCheck;
    public LayerMask wallLayer;
    public float wallSlideSpeed = 2f;
    public Vector2 wallJumpPower = new Vector2(8f, 12f);
    public float wallJumpDuration = 0.2f;

    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpCounter;

    [Header("Main Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Knockback Settings")]
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;

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

    [Header("Audio Settings")]
    public AudioClip footstepSound;
    [Range(0f, 1f)] public float footstepVolume = 0.5f;
    public AudioClip jumpSound;
    [Range(0f, 1f)] public float jumpVolume = 0.5f;
    private AudioSource audioSource;

    private Animator anim;
    private DashTrail dashTrail;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        dashTrail = GetComponent<DashTrail>();

        if (PlayerPrefs.GetInt("HasSaved", 0) == 1)
        {
            float savedX = PlayerPrefs.GetFloat("SavedPosX");
            float savedY = PlayerPrefs.GetFloat("SavedPosY");

            transform.position = new Vector3(savedX, savedY, transform.position.z);

            hasDash = PlayerPrefs.GetInt("HasDash", 0) == 1;
            hasDoubleJump = PlayerPrefs.GetInt("HasDoubleJump", 0) == 1;
            hasWallJump = PlayerPrefs.GetInt("HasWallJump", 0) == 1;

            Debug.Log("Logged position and loaded abilities!");
        }

        lastSafePosition = transform.position;
    }

    void Update()
    {
        if (isDashing || isKnockedBack) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        if (hasDash && Input.GetKeyDown(KeyCode.LeftShift) && canDashFlag)
        {
            StartCoroutine(PerformDash());
            return;
        }

        if (wallCheck != null)
        {
            isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        }

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

        WallSlide();
        WallJump();

        if (Input.GetButtonDown("Jump"))
        {
            if (wallJumpCounter > 0f && hasWallJump)
            {
            }
            else
            {
                jumpBufferCounter = jumpBufferTime;
            }
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && !isWallJumping)
        {
            if (coyoteTimeCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpBufferCounter = 0f;
                PlayJumpSound();
            }
            else if (jumpsLeft > 0 && hasDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpsLeft--;
                jumpBufferCounter = 0f;
                PlayJumpSound();
            }
        }

        if (Input.GetButtonUp("Jump") && !isWallJumping)
        {
            coyoteTimeCounter = 0f;
        }

        if (!isWallJumping)
        {
            Flip();
        }

        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (isDashing || isKnockedBack || isWallJumping) return;

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0 && !isWallSliding)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump") && !isWallSliding)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void WallSlide()
    {
        if (hasWallJump && isTouchingWall && !isGrounded && horizontalInput != 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -Mathf.Sign(transform.localScale.x);
            wallJumpCounter = coyoteTime;
            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpCounter = 0f;

            PlayJumpSound();

            if (Mathf.Sign(transform.localScale.x) != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJump), wallJumpDuration);
        }
    }

    private void StopWallJump()
    {
        isWallJumping = false;
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
        if (dashTrail != null)
            dashTrail.StartTrail(dashDuration);

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

    public void ApplyKnockback(Vector2 sourcePosition)
    {
        if (isKnockedBack) return;
        isKnockedBack = true;
        float direction = transform.position.x < sourcePosition.x ? -1 : 1;
        rb.linearVelocity = new Vector2(direction * knockbackForce, knockbackForce * 0.5f);
        Invoke(nameof(StopKnockback), knockbackDuration); 
    }

    void StopKnockback()
    {
        isKnockedBack = false;
    }

    public void PlayFootstepSound()
    {
        if (isGrounded && audioSource != null && footstepSound != null)
        {
            audioSource.PlayOneShot(footstepSound, footstepVolume);
        }
    }
    public void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound, jumpVolume);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, 0.2f);
        }
    }
}