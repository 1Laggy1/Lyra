using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float jumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)][SerializeField] private float crouchSpeed = .36f;           // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)][SerializeField] private float movementSmoothing = .05f;   // How much to smooth out the movement
    [SerializeField] private bool airControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask whatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform groundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform ceilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D crouchDisableCollider;                // A collider that will be disabled when crouching

    const float KGroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool grounded;            // Whether or not the player is grounded.
    const float KCeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private new Rigidbody2D rigidbody2D;
    private bool facingRight = true;  // For determining which way the player is currently facing.
    private Vector3 velocity = Vector3.zero;
    public float MaxJumpHeight = 10f; // Maximum height the player can reach
    public float JumpSpeed = 2f;      // How fast the player ascends
    public float FallSpeed = 5f;      // How fast the player descends when not holding the jump button
    public float HoverBuffer = 0.2f;  // Slight buffer to smooth transition from rising to falling
    private bool isFlying = false;
    private bool isHoldingJump = false;
    private float startingYPosition;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool wasCrouching = false;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        OnLandEvent ??= new UnityEvent();

        OnCrouchEvent ??= new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, KGroundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
        }
    }

    public void Move(float move, bool crouch, bool jump, float jumpForce)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(ceilingCheck.position, KCeilingRadius, whatIsGround))
            {
                crouch = true;
            }
        }

        // only control the player if grounded or airControl is turned on
        if (grounded || airControl)
        {
            // If crouching
            if (crouch)
            {
                if (!wasCrouching)
                {
                    wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= crouchSpeed;

                // Disable one of the colliders when crouching
                if (crouchDisableCollider != null)
                {
                    crouchDisableCollider.enabled = false;
                }
            }
            else
            {
                // Enable the collider when not crouching
                if (crouchDisableCollider != null)
                {
                    crouchDisableCollider.enabled = true;
                }

                if (wasCrouching)
                {
                    wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, rigidbody2D.velocity.y);

            // And then smoothing it out and applying it to the character
            rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }

            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            StartFlight();
        }

        // While the jump button is held, continue flight if below the max height
        if (isFlying && isHoldingJump && transform.position.y < startingYPosition + MaxJumpHeight)
        {
            FlyUpward();
        }
        else if (isFlying && (!isHoldingJump || transform.position.y >= startingYPosition + MaxJumpHeight))
        {
            StopFlight();
        }

        // Check for release of the jump button
        if (Input.GetButtonUp("Jump"))
        {
            isHoldingJump = false;
        }
    }

    void StartFlight()
    {
        isFlying = true;
        isHoldingJump = true;
        startingYPosition = transform.position.y; // Set the current Y position as the baseline
        Debug.Log("Flight started");
    }

    void FlyUpward()
    {
        // Gradually increase Y position as long as the jump button is held and max height is not reached
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, JumpSpeed); // Ascend with jump speed
        Debug.Log("Flying upwards");
    }

    void StopFlight()
    {
        isFlying = false;

        // Apply a slight hover effect before falling
        // m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -fallSpeed);
        Debug.Log("Flight stopped, falling");
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
