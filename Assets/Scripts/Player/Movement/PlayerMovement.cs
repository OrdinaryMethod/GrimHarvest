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
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

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
        ClimbSwitch();
        FixTheBugs();
        Move();

        

        if (grabbingLedge)
        {
            animState = "grabbingLedge";
            canMove = false;
            Debug.Log(animState + "movement controller");
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

        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        jump();
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
        if (Input.GetKeyDown(jumpKey) && isGrounded && !grabbingLedge && !canClimb && canJump) //Normal jump
        {
            rb2d.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
        }
        else if(Input.GetKeyDown(jumpKey) && !isGrounded && grabbingLedge && canClimb) //Ledge jump
        {
            grabbingLedge = false;
            rb2d.constraints = RigidbodyConstraints2D.None;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

            Collider2D ledgeCollider = GetComponentInChildren<GrabPoint>().ledgeCollider;

            //Get colliders to ignore
            Physics2D.IgnoreCollision(ledgeCollider, GetComponent<Collider2D>(), true);

            canJump = false;

            StartCoroutine(ClimbUpLedge());
        }

        
    }

    IEnumerator ClimbUpLedge()
    {
        isClimbing = true;
        canMove = false;
        canFlip = false;

        //Assign backup variables
        float rbMass = rb2d.mass;
        float rbGravity = rb2d.gravityScale;

        //Set player values to 0
        rb2d.mass = 0;
        rb2d.gravityScale = 0;
        climbingPoints = true;

        //Move to points
        setClimbPoint = 1;
        yield return new WaitForSeconds(climbingCooldown);
        canMove = false;
        setClimbPoint = 2;
        yield return new WaitForSeconds(climbingCooldown);
        canMove = false;
        setClimbPoint = 3;
        yield return new WaitForSeconds(climbingCooldown);
        setClimbPoint = 4;

        //Re-assign rb values
        rb2d.mass = rbMass;
        rb2d.gravityScale = rbGravity;
        isClimbing = false;

        yield return new WaitForSeconds(0.2f);
        climbingPoints = false;
        canMove = true;
        canJump = true;
        canFlip = true;
        setClimbPoint = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
      
    }

    //Minor functions
    private void ClimbSwitch()
    {
        if(climbingPoints)
        {
            switch (setClimbPoint)
            {
                case 1:
                    gameObject.transform.position = Vector2.MoveTowards(transform.position, climbPoints[0], climbingSpeed * Time.deltaTime);
                    break;
                case 2:
                    gameObject.transform.position = Vector2.MoveTowards(transform.position, climbPoints[1], climbingSpeed * Time.deltaTime);
                    break;
                case 3:
                    gameObject.transform.position = Vector2.MoveTowards(transform.position, climbPoints[2], climbingSpeed * Time.deltaTime);
                    break;
                case 4:
                    climbPoints.Clear();
                    break;
            };
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

    private void FixTheBugs()
    {
        //Prevent gripping bugs
        if (isGrounded)
        {
            canClimb = false;
        }

        if (isGrounded && grabbingLedge)
        {
            isGrounded = false;
            canClimb = true;
        }
    }
}
