using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbing : MonoBehaviour
{
    private PlayerController _playerController;
    private Keybinds _keyBinds;
    private Rigidbody2D _rb2d;

    [Header("Settings")]
    public bool wallSliding;
    [Range(0.0f, 25.0f)]
    [SerializeField] private float _wallSlidingSpeed;
    [SerializeField] private bool _wallJumping;
    [Range(0.0f, 100.0f)]
    [SerializeField] private float _xWallForce;
    [Range(0.0f, 100.0f)]
    [SerializeField] private float _yWallForce;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _wallJumpTime;

    public bool isTouchingFront;
    [SerializeField] private bool _isGrounded;
 
    [Header("Keybinds")]
    private KeyCode _jump; 

    // Start is called before the first frame update
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get Keybinds
        _keyBinds = gameObject.GetComponent<Keybinds>();

        isTouchingFront = _playerController.isTouchingFront;
        _isGrounded = _playerController.isGrounded;

        if(!_playerController.droidActive)
        {
            MapKeybinds();
            WallSliding();
            WallJumping();
        }
    }

    void FixedUpdate()
    {
        if (_wallJumping)
        {
            float wallJumpMultiplier;
            if (_playerController.facingRight)
            {
                wallJumpMultiplier = 0.5f;
            }
            else
            {
                wallJumpMultiplier = -0.5f;
            }

            _rb2d.velocity = new Vector2(_xWallForce * -wallJumpMultiplier, _yWallForce);
        }
    }

    void MapKeybinds()
    {
        _jump = _keyBinds.jump;
    }

    void WallSliding()
    {
        if (isTouchingFront && !_isGrounded)
        {
            wallSliding = true;

            //State
            _playerController.isClimbing = true;
        }
        else
        {
            wallSliding = false;

            //State
            _playerController.isClimbing = false;
        }

        if (wallSliding)
        {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, Mathf.Clamp(_rb2d.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
    }

    void WallJumping()
    {
        if (Input.GetKeyDown(_jump) && wallSliding)
        {
            _wallJumping = true;
            Invoke("SetWallJumpingToFalse", _wallJumpTime);
        }

        
    }

    void SetWallJumpingToFalse()
    {
        _wallJumping = false;
    }
}
