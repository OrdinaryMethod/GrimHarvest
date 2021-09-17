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

    const string Player_Idle = "Player_Idle";
    const string Player_Run = "Player_Run";
    const string Player_Jump = "Player_Jump";
    const string Player_Land = "Player_Land";

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
    }

    private void AnimChanger()
    {
        if (canChangeAnim)
        {
            switch (playerMovement.animState)
            {
                case "idle":
                    Debug.Log("idle");
                    playerAnim.Play(Player_Idle);
                    break;
                case "running":
                    Debug.Log("running");
                    playerAnim.Play(Player_Run);
                    break;
                case "jumping":
                    Debug.Log("jumping");
                    playerAnim.Play(Player_Jump);
                    break;
                case "landing":
                    canChangeAnim = false;
                    StartCoroutine(DelayToLand());
                    playerAnim.Play(Player_Land);
                    break;
                case "":
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
}
