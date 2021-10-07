using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    Keybinds keybinds;
    PlayerStats playerStats;

    public Vector2 playerPosition;
    public List<Vector2> climbPoints = new List<Vector2>();
    private Rigidbody2D rb2d;
    private float playerSpeed;
    private float moveHorizontal;
    public bool facingRight;
    public bool canFlip;

    [Header("Movement Conditions")]
    public bool isGrounded;
    public bool grabbingLedge;
    public bool canMove;
    public bool canClimb;

    [Header("Jumping Values")]
    private bool canJump;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    [Header("Climbing Values")]
    [SerializeField] private float climbingSpeed;
    [SerializeField] private float climbingCooldown;
    private bool climbingPoints;
    private int setClimbPoint;

    [Header("Animation Values")]
    public string animState;
    public bool isClimbing;
    private bool isCrouching;

    //Keybinds
    private KeyCode jumpKey;
    private KeyCode crouchKey;
    private KeyCode aimUpKey;
    private KeyCode aimDownKey;

    void Awake()
    {
        //Object Components
        rb2d = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>(); 

        //Set base Stats
        playerSpeed = playerStats.playerSpeed;

        //Set base values
        canMove = true;
        canClimb = false;
        canJump = true;
        canFlip = true;
        isClimbing = false;
        isCrouching = false;

        //Initial collision ingores
        Physics2D.IgnoreLayerCollision(0, 9, true);
    }

    void Update()
    {
        GetKeyBinds();     
        Crouch();
        Move();
        FixTheBugs();

        //if (rb2d.velocity.y < 0)
        //{
        //    rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        //}
        //else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
        //{
        //    rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        //}

        jump();


        if (grabbingLedge)
        {
            animState = "grabbingLedge";
        }
        else if(isClimbing)
        {
            animState = "climbing";
        }

        if (!isGrounded && !Input.GetKey(aimUpKey) && !Input.GetKey(aimDownKey) && !grabbingLedge)
        {
            animState = "jumping";
        }
        else if (!isGrounded && Input.GetKey(aimUpKey) && !Input.GetKey(aimDownKey) && !grabbingLedge)
        {
            animState = "jumpingAimingUp";
        }
        else if (!isGrounded && !Input.GetKey(aimUpKey) && Input.GetKey(aimDownKey) && !grabbingLedge)
        {
            animState = "jumpingAimingDown";
        }
    }

    void FixedUpdate()
    {
        if(!isCrouching)
        {
            if(canMove)
            {
                Vector2 movement = new Vector2(moveHorizontal * playerSpeed, rb2d.velocity.y);
                rb2d.velocity = movement;
            }          
        }       
    }

    private void GetKeyBinds()
    {
        keybinds = GetComponent<Keybinds>(); //Define 

        //Assign
        jumpKey = keybinds.jump;
        crouchKey = keybinds.crouch;
        aimUpKey = keybinds.aimUp;
        aimDownKey = keybinds.aimDown;
    }

    private void Move()
    {
        if(canMove)
        {
            //Check for horizontal movement
            if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
            {      
                if(isGrounded && !Input.GetKey(aimUpKey) && !Input.GetKey(aimDownKey))
                {
                    animState = "running";
                }
                else if(isGrounded && Input.GetKey(aimUpKey) && !Input.GetKey(aimDownKey))
                {
                    animState = "runningAimingUp";
                }
                else if(isGrounded && !Input.GetKey(aimUpKey) && Input.GetKey(aimDownKey))
                {
                    animState = "runningAimingDown";
                }

                if (Input.GetAxisRaw("Horizontal") > 0.5f && !facingRight && !canClimb)
                {
                    //If we're moving right but not facing right, flip the sprite and set     facingRight to true.
                    if(canFlip)
                    {
                        Flip();
                        facingRight = true;
                    }                   
                }
                else if (Input.GetAxisRaw("Horizontal") < 0.5f && facingRight && !canClimb)
                {
                    //If we're moving left but not facing left, flip the sprite and set facingRight to false.
                    if (canFlip)
                    {
                        Flip();
                        facingRight = false;
                    }
                }
            }
            else
            {
                if(isGrounded && !Input.GetKey(aimUpKey) && !Input.GetKey(aimDownKey))
                {
                    animState = "idle";
                } 
                else if(isGrounded && Input.GetKey(aimUpKey) && !Input.GetKey(aimDownKey))
                {
                    animState = "idleAimingUp";
                }
                else if(isGrounded && !Input.GetKey(aimUpKey) && Input.GetKey(aimDownKey))
                {
                    animState = "idleAimingDown";
                }
                else if (isGrounded && Input.GetKey(aimUpKey) && Input.GetKey(aimDownKey))
                {
                    animState = "idle";
                }
            }

             moveHorizontal = Input.GetAxisRaw("Horizontal");
        }
    }

    private void Crouch()
    {
        
        if(Input.GetKey(crouchKey) && isGrounded && !Input.GetKey(aimUpKey) && !Input.GetKey(aimDownKey))
        {
            canMove = false;
            canJump = false;
            isCrouching = true;
            animState = "crouching";
            rb2d.velocity = new Vector2(0, 0);
        }
        else if(Input.GetKey(crouchKey) && isGrounded && Input.GetKey(aimUpKey) && !Input.GetKey(aimDownKey))
        {
            canMove = false;
            canJump = false;
            isCrouching = true;
            animState = "crouchingAimingUp";
            rb2d.velocity = new Vector2(0, 0);
        }
        else if(Input.GetKey(crouchKey) && isGrounded && !Input.GetKey(aimUpKey) && Input.GetKey(aimDownKey))
        {
            canMove = false;
            canJump = false;
            isCrouching = true;
            animState = "crouchingAimingDown";
            rb2d.velocity = new Vector2(0, 0);
        }
        else
        {
            canMove = true;
            
            if(isCrouching)
            {
                animState = "idle";
                isCrouching = false;
                canJump = true;
            }
        }
    }

    private void jump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded && !grabbingLedge) //Normal jump
        {
            rb2d.AddForce(Vector2.up * 25f, ForceMode2D.Impulse);
        }
        else if(Input.GetKeyDown(jumpKey) && !isGrounded && grabbingLedge) //Ledge jump
        {
            rb2d.AddForce(Vector2.up * 25f, ForceMode2D.Impulse);
        }   
    }

    //Minor functions
    void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void FixTheBugs()
    {
        if(isGrounded)
        {
            grabbingLedge = false;
        }
    }
}
