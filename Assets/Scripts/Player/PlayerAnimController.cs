using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerCombatController playerCombatController;
    private Animator playerAnimator;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerCombatController = GetComponent<PlayerCombatController>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        //Shooting
        if (playerCombatController.isShooting)
        {
            playerAnimator.SetBool("isShooting", true);
            playerCombatController.isShooting = false;
        }
        else
        {
            playerAnimator.SetBool("isShooting", false);
        }

        //Running
        if(playerController.isRunning)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
    }
}
