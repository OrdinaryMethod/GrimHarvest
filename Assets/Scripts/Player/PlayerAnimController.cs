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

    const string Player_Idle = "Player_Idle";
    const string Player_Run = "Player_Run";
    const string Player_Jump = "Player_Jump";
    const string Player_Land = "Player_Land";
    const string Player_GrabLedge = "Player_GrabLedge";
    const string Player_Climb = "Player_Climb";
    const string Player_Crouch = "Player_Crouch";

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        groundingPoint = GetComponentInChildren<GroundingPoint>();
        playerAnim = GetComponent<Animator>();

        canChangeAnim = true;
    }

    void Update()
    {
        AnimChanger();
        CheckLanding();
        CheckClimbing();
    }

    private void AnimChanger()
    {
        if (canChangeAnim)
        {
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
