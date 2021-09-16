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

    [Header("Movement Conditions")]
    public bool isGrounded;
    public bool grabbingLedge;
    public bool canMove;
    public bool canClimb;

    [Header("Jumping Values")]
    [SerializeField] private float normalJump;
    [SerializeField] public float jumpCooldown;

    private bool canJump;
    

    [Header("Climbing Values")]
    [SerializeField] private float climbingSpeed;
    [SerializeField] private float climbingCooldown;

    private bool climbingPoints;
    private int setClimbPoint;

    [Header("Animation Values")]
    public bool isJumping;
    public bool canFlip;

    //Keybinds
    private KeyCode jumpKey;

    void Start()
    {
        //Object Components
        rb2d = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>(); //This must only be set on start as game mechanics can temporarily change these

        //Set base Stats
        playerSpeed = playerStats.playerSpeed;

        //Set base values
        canMove = true;
        canClimb = false;
        canJump = true;
        canFlip = true;

        //Initial collision ingores
        Physics2D.IgnoreLayerCollision(0, 9, true);
    }

    void Update()
    {
        GetKeyBinds();
        Move();
        jump();
        ClimbSwitch();
        FixTheBugs();      
    }

    private void GetKeyBinds()
    {
        keybinds = GetComponent<Keybinds>(); //Define 

        //Assign
        jumpKey = keybinds.jump;
    }

    //Movement controls
    private void Move()
    {
        if(canMove)
        {
            //Check for horizontal movement
            if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
            {
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

            moveHorizontal = Input.GetAxis("Horizontal") * playerSpeed;
            Vector2 Movement = new Vector2(moveHorizontal, 0);
            Movement *= Time.deltaTime;
            rb2d.transform.Translate(Movement);
            playerPosition = rb2d.position;
        }
    }

    private void jump()
    {
        //big 
        if (Input.GetKeyDown(jumpKey) && isGrounded && !grabbingLedge && !canClimb && canJump) //Normal jump
        {
            rb2d.AddForce(new Vector2(0f, normalJump), ForceMode2D.Impulse);
            isGrounded = false;
            isJumping = true;
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
        canMove = false;
        canFlip = false;

        //Assign backup variables
        float rbMass = rb2d.mass;
        float rbGravity = rb2d.gravityScale;

        //Set player values to 0
        rb2d.mass = 0;
        rb2d.gravityScale = 0;
        climbingPoints = true;

        //Tell climbing animation to start
        GetComponent<PlayerAnimController>().isClimbing = true;
        //Move to points
        setClimbPoint = 1;
        yield return new WaitForSeconds(climbingCooldown);
        canMove = false;
        setClimbPoint = 2;
        yield return new WaitForSeconds(climbingCooldown);
        canMove = false;
        setClimbPoint = 3;
        yield return new WaitForSeconds(climbingCooldown);
        setClimbPoint = 0;   

        //Re-assign rb values
        rb2d.mass = rbMass;
        rb2d.gravityScale = rbGravity;

        yield return new WaitForSeconds(0.2f);
        climbingPoints = false;
        canMove = true;
        canJump = true;
        canFlip = true;
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
