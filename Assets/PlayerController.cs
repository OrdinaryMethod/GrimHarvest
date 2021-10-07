using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb2d;
    public Transform frontCheck;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    [Header("Running")]
    private float moveInput;
    public float speed;

    [Header("Jumping")]
    public float jumpForce;
    private bool isGrounded;
    bool isTouchingFront;
    bool wallSliding;
    public float wallSlidingSpeed;
    private bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    [Header("Misc")]
    public bool facingRight;
    public float checkRadius;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Get movement input
        moveInput = Input.GetAxisRaw("Horizontal");

        Running();
        Jumping();
        WallSliding();
        WallJumping();

        //Flip character
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Improve jumping
        NotFloatyJump();
    }

    void Running()
    {
        rb2d.velocity = new Vector2(moveInput * speed, rb2d.velocity.y);
    }

    void Jumping()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2d.velocity = Vector2.up * jumpForce;
        }
    }

    void WallSliding()
    {
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);

        if (isTouchingFront && !isGrounded && moveInput != 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (wallSliding)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }

    void WallJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && wallSliding)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (wallJumping)
        {
            rb2d.velocity = new Vector2(xWallForce * -moveInput, yWallForce);
        }
    }

    void NotFloatyJump()
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (10.5f - 1) * Time.deltaTime;
        }
        else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (10f - 1) * Time.deltaTime;
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }
}
