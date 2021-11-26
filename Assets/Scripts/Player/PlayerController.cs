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
    private Keybinds keyBinds;
    public VectorValue startingPos;

    [Header("Droid")]
    public bool droidActive;

    [Header("Running")]
    public bool canMove;
    private float moveInput;
    public float speed;
    public float sprintSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public bool isGrounded;
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
    public float angle;

    [Header("Aiming")]
    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform head;

    [Header("Animation State")]
    public string animState;
    bool hasLanded;

    [Header("Keybinds")]
    KeyCode jump;
    KeyCode sprint;
    KeyCode aim;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        droidActive = false;
        canMove = true;


        //Values from scriptable object
        transform.position = startingPos.initialValue;
        if(!startingPos.facingRight)
        {
            Flip();
        }
    }

    void Update()
    {
        //Get Keybinds
        keyBinds = gameObject.GetComponent<Keybinds>();

        if(!droidActive)
        {
            //Get movement input
            moveInput = Input.GetAxisRaw("Horizontal");

            MapKeybinds();
            AnimationState();
            if(canMove)
            {
                Running();
                Jumping();
            }         
            WallSliding();
            WallJumping();
            AimDirection();

            //Flip character
            if(canMove)
            {
                if (moveInput > 0 && !facingRight)
                {
                    Flip();
                }
                else if (moveInput < 0 && facingRight)
                {
                    Flip();
                }
            }            
        }   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        NotFloatyJump();
    }

    void MapKeybinds()
    {
        jump = keyBinds.jump;
        sprint = keyBinds.sprint;
        aim = keyBinds.aim;
    }

    void Running()
    {
        float currentSpeed;

        if(Input.GetKey(sprint))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = speed;
        }

        rb2d.velocity = new Vector2(moveInput * currentSpeed, rb2d.velocity.y);
    }

    void Jumping()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetKeyDown(jump) && isGrounded)
        {
            rb2d.velocity = Vector2.up * jumpForce;
        }
    }

    void WallSliding()
    {
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);

        if (isTouchingFront && !isGrounded)
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
        if (Input.GetKeyDown(jump) && wallSliding)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (wallJumping)
        {
            float wallJumpMultiplier;
            if(facingRight)
            {
                wallJumpMultiplier = 0.5f;
            }
            else
            {
                wallJumpMultiplier = -0.5f;
            }


            rb2d.velocity = new Vector2(xWallForce * -wallJumpMultiplier, yWallForce);
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

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }

    void AimDirection()
    {
        if (Input.GetKey(aim))
        {
            canMove = false;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y); //Prevents slowfall

            Vector2 leftArmDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - leftArm.position;
            Vector2 rightArmDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - rightArm.position;
            Vector2 headDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - head.position;

            float leftArmAngle = (Mathf.Atan2(leftArmDirection.y, leftArmDirection.x) * Mathf.Rad2Deg);
            float rightArmAngle = (Mathf.Atan2(rightArmDirection.y, rightArmDirection.x) * Mathf.Rad2Deg);
            float headAngle = (Mathf.Atan2(headDirection.y, headDirection.x) * Mathf.Rad2Deg);

            if (!facingRight)
            {
                leftArmAngle = leftArmAngle - 180;
                rightArmAngle = rightArmAngle - 180;
                headAngle = headAngle - 180;
            }
  
            Quaternion leftArmRotation = Quaternion.AngleAxis(leftArmAngle, Vector3.forward);
            Quaternion rightArmRotation = Quaternion.AngleAxis(rightArmAngle, Vector3.forward);
            Quaternion headRotation = Quaternion.AngleAxis(headAngle, Vector3.forward);

            leftArm.rotation = Quaternion.Slerp(leftArm.rotation, leftArmRotation, 50 * Time.deltaTime);
            rightArm.rotation = Quaternion.Slerp(rightArm.rotation, rightArmRotation, 50 * Time.deltaTime);
            head.rotation = Quaternion.Slerp(head.rotation, headRotation, 50 * Time.deltaTime);

            if (facingRight && leftArmDirection.x < 0)
            {
                Flip();
            }
            else if (!facingRight && leftArmDirection.x > 0)
            {
                Flip();
            }
        }
        else
        {
            canMove = true;
            leftArm.rotation = Quaternion.Euler(0, 0, 0);
            rightArm.rotation = Quaternion.Euler(0, 0, 0);
            head.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void AnimationState()
    {
        //Running & Idle
        if (isGrounded)
        {
            if (moveInput > 0 || moveInput < 0)
            {
                animState = "running";
            }
            else if (moveInput == 0)
            {
                animState = "idle";
            }
        }
        else
        {
            animState = "jumping";
        }

        if (hasLanded)
        {
            hasLanded = false;
            animState = "landing";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Landing
        if (collision.gameObject.name == "Tilemap_Ground")
        {
            hasLanded = true;
        }
    }
}
