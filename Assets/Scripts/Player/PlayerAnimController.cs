using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    PlayerMovement playerMovement;
    GroundingPoint groundingPoint;
    private Animator playerAnim;

    private bool canChangeAnim;
    private bool hasLanded;
    private bool isClimbing;

    //Base Animations
    const string Player_Idle = "Player_Idle";
    const string Player_Run = "Player_Run";
    const string Player_Jump = "Player_Jump";
    const string Player_Land = "Player_Land";
    const string Player_GrabLedge = "Player_GrabLedge";
    const string Player_Climb = "Player_Climb";
    const string Player_Crouch = "Player_Crouch";
    const string Player_Idle_AimUp = "Player_Idle_AimUp";
    const string Player_Run_AimUp = "Player_Run_AimUp";
    const string Player_Jump_AimUp = "Player_Jump_AimUp";
    const string Player_Crouch_AimUp = "Player_Crouch_AimUp";
    const string Player_Idle_AimDown = "Player_Idle_AimDown";
    const string Player_Run_AimDown = "Player_Run_AimDown";
    const string Player_Jump_AimDown = "Player_Jump_AimDown";
    const string Player_Crouch_AimDown = "Player_Crouch_AimDown";

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        groundingPoint = GetComponentInChildren<GroundingPoint>();
        playerAnim = GetComponent<Animator>();

        canChangeAnim = true;
    }

    void Update()
    {
        BaseAnimChanger();
        CheckLanding();
        CheckClimbing();
    }

    private void BaseAnimChanger()
    {
        if (canChangeAnim)
        {
            string jumpAimDirection;

            switch (playerMovement.animState)
            {
                case "idle":
                    playerAnim.Play(Player_Idle);
                    break;
                case "running":
                    playerAnim.Play(Player_Run);
                    break;
                case "jumping":
                    playerAnim.Play(Player_Jump);
                    break;
                case "landing":
                    canChangeAnim = false;
                    StartCoroutine(DelayToLand());
                    playerAnim.Play(Player_Land);
                    break;
                case "grabbingLedge":
                    playerAnim.Play(Player_GrabLedge);
                    break;
                case "climbing":
                    canChangeAnim = false;
                    StartCoroutine(DelayToClimb());
                    playerAnim.Play(Player_Climb);
                    break;
                case "crouching":
                    playerAnim.Play(Player_Crouch);
                    break;
                case "idleAimingUp":
                    playerAnim.Play(Player_Idle_AimUp);
                    break;
                case "runningAimingUp":
                    playerAnim.Play(Player_Run_AimUp);
                    break;
                case "jumpingAimingUp":
                    if(hasLanded)
                    {
                        canChangeAnim = false;
                        jumpAimDirection = "up";
                        playerAnim.Play(Player_Land);
                        StartCoroutine(AimUpJumpDelay(jumpAimDirection));
                    }
                    else
                    {
                        playerAnim.Play(Player_Jump_AimUp);
                    }              
                    break;
                case "crouchingAimingUp":
                    playerAnim.Play(Player_Crouch_AimUp);
                    break;
                case "idleAimingDown":
                    playerAnim.Play(Player_Idle_AimDown);
                    break;
                case "runningAimingDown":
                    playerAnim.Play(Player_Run_AimDown);
                    break;
                case "jumpingAimingDown":
                    if (hasLanded)
                    {
                        canChangeAnim = false;
                        jumpAimDirection = "down";
                        playerAnim.Play(Player_Land);
                        StartCoroutine(AimUpJumpDelay(jumpAimDirection));
                    }
                    else
                    {
                        playerAnim.Play(Player_Jump_AimDown);
                    }
                    break;
                case "crouchingAimingDown":
                    playerAnim.Play(Player_Crouch_AimDown);
                    break;

            }
        }
    }

    private void CheckLanding()
    {
        hasLanded = groundingPoint.landed;
        if(hasLanded)
        {
            playerMovement.animState = "landing";
        }
    }

    IEnumerator AimUpJumpDelay(string aimDirection)
    {
        yield return new WaitForSeconds(0.1f);
        if(aimDirection == "up")
        {
            playerAnim.Play(Player_Jump_AimUp);
        }
        else if(aimDirection == "down")
        {
            playerAnim.Play(Player_Jump_AimDown);
        }
        canChangeAnim = true;
    }

    IEnumerator DelayToLand()
    {
        yield return new WaitForSeconds(0.1f);
        canChangeAnim = true;
    }

    private void CheckClimbing()
    {
        isClimbing = playerMovement.isClimbing;
        if(isClimbing)
        {
            playerMovement.animState = "climbing";
        }
    }

    IEnumerator DelayToClimb()
    {
        yield return new WaitForSeconds(0.05f);
        canChangeAnim = true;
    }
}
