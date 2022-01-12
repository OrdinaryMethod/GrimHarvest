using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb2d;
    public Transform frontCheck;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private Keybinds _keyBinds;
    public VectorValue startingPos;

    [Header("Droid")]
    public bool droidActive;

    [Header("Running")]
    public bool canMove;
    private float _moveInput;
    [Range(0.0f, 100.0f)]
    public float speed;

    [Header("Jumping")]
    [Range(0.0f, 100.0f)]
    public float jumpForce;
    [SerializeField] private float _setJumpCooldown;
    [SerializeField] private float _setGroundedCooldown;
    private float _jumpCooldown;
    private float _groundedCooldown;
    public bool isGrounded;
    [HideInInspector] public bool isTouchingFront;

    [Header("States")]
    public bool isRunning;
    public bool isSprinting;
    public bool isJumping;
    public bool isClimbing;
    public bool isCrouching;
    public bool isHidden;

    [Header("Misc")]
    public bool facingRight;
    public float checkRadius;
    public float angle;

    [Header("Aiming")]
    [SerializeField] private Transform _rightArm;
    [SerializeField] private Transform _leftArm;
    [SerializeField] private Transform _head;

    [Header("Keybinds")]
    private KeyCode _jump;
    private KeyCode _aim;

    void Awake()
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
        _keyBinds = gameObject.GetComponent<Keybinds>();

        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);

        if (!droidActive)
        {
            //Get movement input
            _moveInput = Input.GetAxisRaw("Horizontal");

            MapKeybinds();
            if(canMove)
            {
                Running();
                Jumping();
            }
            Crouching();
            AimDirection();

            //Flip character
            if(canMove)
            {
                if (_moveInput > 0 && !facingRight)
                {
                    Flip();
                }
                else if (_moveInput < 0 && facingRight)
                {
                    Flip();
                }
            }            
        }   
    }

    void FixedUpdate()
    {
        NotFloatyJump();
    }

    void MapKeybinds()
    {
        _jump = _keyBinds.jump;
        _aim = _keyBinds.aim;
    }

    void Running()
    {
        //State
        if((Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") < 0) && isGrounded)
        {
            isRunning = true;
            
        }
        else
        {
            isRunning = false;
        }

        rb2d.velocity = new Vector2(_moveInput * speed, rb2d.velocity.y);
    }

    void Jumping()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, transform.localScale, 0, whatIsGround);

        _groundedCooldown -= Time.deltaTime;

        if (isGrounded)
        {
            _jumpCooldown -= Time.deltaTime;

            _groundedCooldown = _setGroundedCooldown;
        }

        if (Input.GetKeyDown(_jump) && _jumpCooldown <= 0 && _groundedCooldown > 0)
        {
            _jumpCooldown = _setJumpCooldown;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 1 * jumpForce);
        }

        //State
        if (!isGrounded && !isClimbing)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }

    void Crouching()
    {
        if(Input.GetKey(KeyCode.C))
        {
            isCrouching = true; //State
        }
        else
        {
            isCrouching = false; //State
        }
    }

    void NotFloatyJump()
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (12f - 1) * Time.deltaTime;
        }
        else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (20f - 1) * Time.deltaTime;
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

    void AimDirection()
    {
        if (Input.GetKey(_aim))
        {
            canMove = false;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y); //Prevents slowfall

            Vector2 leftArmDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _leftArm.position;
            Vector2 rightArmDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _rightArm.position;
            Vector2 headDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _head.position;

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

            _leftArm.rotation = Quaternion.Slerp(_leftArm.rotation, leftArmRotation, 50 * Time.deltaTime);
            _rightArm.rotation = Quaternion.Slerp(_rightArm.rotation, rightArmRotation, 50 * Time.deltaTime);
            _head.rotation = Quaternion.Slerp(_head.rotation, headRotation, 50 * Time.deltaTime);

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
            _leftArm.rotation = Quaternion.Euler(0, 0, 0);
            _rightArm.rotation = Quaternion.Euler(0, 0, 0);
            _head.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
