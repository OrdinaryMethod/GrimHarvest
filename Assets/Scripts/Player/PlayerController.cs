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

    [SerializeField] private Transform rArmDefault;
    [SerializeField] private Transform lArmDefault;
    [SerializeField] private Transform headDefault;

    [Header("Animation State")]
    public string animState;
    bool hasLanded;

    [Header("Keybinds")]
    KeyCode jump;

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
        if (Input.GetKey(KeyCode.Mouse1))
        {
            canMove = false;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y); //Prevents slowfall

            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameObject.Find("LArm").GetComponent<Transform>().position;
             
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            if(!facingRight)
            {
                angle = angle - 180;
            }
  
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
          
            GameObject.Find("LArm").GetComponent<Transform>().rotation = Quaternion.Slerp(GameObject.Find("LArm").GetComponent<Transform>().rotation, rotation, 50 * Time.deltaTime);
        }
        else
        {
            canMove = true;
            GameObject.Find("LArm").GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void MapKeybinds()
    {
        jump = keyBinds.jump;
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
