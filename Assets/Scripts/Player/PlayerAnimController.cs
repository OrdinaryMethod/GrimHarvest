using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator playerAnim;

    [Header("Animation Conditions")]
    public bool isRunning;


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeys();
    }

    private void CheckKeys()
    {
        //Running idle
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            playerAnim.SetBool("running", true);
            isRunning = true;
        }
        else
        {
            playerAnim.SetBool("running", false);
            isRunning = false;
        }

        //Jumping & climbing
        bool isGrounded = GetComponent<PlayerMovement>().isGrounded;
        bool canClimb = GetComponent<PlayerMovement>().canClimb;

        if(!isGrounded && !canClimb )
        {
            playerAnim.SetBool("jumping", true);
        }
        else if(isGrounded && !canClimb)
        {
            playerAnim.SetBool("jumping", false);
        }
    }
}
