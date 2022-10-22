using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerCombatController _playerCombatController;
    private PlayerMonitor _playerMonitor;
    private Animator _playerAnimator;
    private WallClimbing _wallClimbing;

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCombatController = GetComponent<PlayerCombatController>();
        _playerMonitor = GetComponent<PlayerMonitor>();
        _playerAnimator = GetComponent<Animator>();
        _wallClimbing = GetComponent<WallClimbing>();
    }

    void Update()
    {
        //Shooting
        if (_playerCombatController.isShooting)
        {
            _playerAnimator.SetBool("isShooting", true);
            _playerCombatController.isShooting = false;
        }
        else
        {
            _playerAnimator.SetBool("isShooting", false);
        }

        //Melee
        if(_playerCombatController.isMelee)
        {
            _playerAnimator.SetBool("isMelee", true);
            _playerCombatController.isMelee = false;
        }
        else
        {
            _playerAnimator.SetBool("isMelee", false);
        }

        //Running
        if(_playerController.isRunning)
        {
            _playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            _playerAnimator.SetBool("isRunning", false);
        }

        //Jumping
        if(!_playerController.isGrounded && _playerController.isJumping)
        {
            _playerAnimator.SetBool("isGrounded", false);
            _playerAnimator.SetBool("isJumping", true);
        }
        else
        {
            _playerAnimator.SetBool("isGrounded", true);
            _playerAnimator.SetBool("isJumping", false);
        }

        //Free fall
        if(!_playerController.isGrounded && !_playerController.isJumping)
        {
            _playerAnimator.SetBool("isFreeFalling", true);
            _playerAnimator.SetBool("isGrounded", false);
        }
        else
        {
            _playerAnimator.SetBool("isFreeFalling", false);
            _playerAnimator.SetBool("isGrounded", true);
        }

        //Crouching
        if(_playerController.isCrouching)
        {
            _playerAnimator.SetBool("isCrouching", true);
        }
        else
        {
            _playerAnimator.SetBool("isCrouching", false);
        }

        //Wall Grabbing
        if(_wallClimbing.wallSliding)
        {
            _playerAnimator.SetBool("isWallGrabbing", true);
        }
        else
        {
            _playerAnimator.SetBool("isWallGrabbing", false);
        }

        //Wall Jumping
        if(_wallClimbing.wallJumping)
        {
            _playerAnimator.SetBool("isWallJumping", true);
        }
        else
        {
            _playerAnimator.SetBool("isWallJumping", false);
        }

        //Stopped
        if(_playerController.isTouchingFront && _playerController.isGrounded)
        {
            _playerAnimator.SetBool("isStopped", true);
        }
        else
        {
            _playerAnimator.SetBool("isStopped", false);
        }

        //Sanity Death
        if (_playerMonitor.playerIsInsane)
        {
            _playerAnimator.SetBool("isInsane", true);
        }
        else
        {
            _playerAnimator.SetBool("isInsane", false);
        }

        if(_playerController.isHidden)
        {
            _playerAnimator.SetBool("isHiding", true);
        }
        else
        {
            _playerAnimator.SetBool("isHiding", false);
        }      
    }
}
