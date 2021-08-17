using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    Keybinds keybinds;
    PlayerStats playerStats;

    public Vector2 playerPosition;

    private Rigidbody2D rb2d;

    private float playerSpeed;
    private float moveHorizontal;

    [HideInInspector] public bool facingRight;

    [Header("Movement Conditions")]
    public bool isGrounded;
    public bool grabbingLedge;
    public bool canMove;
    public bool canClimb;

    

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

        //Initial collision ingores
        Physics2D.IgnoreLayerCollision(0, 9, true);

    }

    void Update()
    {
        GetKeyBinds();
        Move();
        jump();
        FixTheBugs();


        //Player Movement. Check for horizontal movement
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5f && !facingRight)
            {
                //If we're moving right but not facing right, flip the sprite and set     facingRight to true.
                Flip();
                facingRight = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0.5f && facingRight)
            {
                //If we're moving left but not facing left, flip the sprite and set facingRight to false.
                Flip();
                facingRight = false;
            }

        }

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
        if (Input.GetKeyDown(jumpKey) && isGrounded && !grabbingLedge && !canClimb) //Normal jump
        {
            rb2d.AddForce(new Vector2(0f, 12f), ForceMode2D.Impulse);
        }
        else if(Input.GetKey(jumpKey) && !isGrounded && grabbingLedge && canClimb) //Ledge jump
        {
            rb2d.AddForce(new Vector2(0f, 15f), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Jumping
        if (collision.gameObject.CompareTag("Surface"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Jumping
        if (collision.gameObject.CompareTag("Surface"))
        {
            isGrounded = false;
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
