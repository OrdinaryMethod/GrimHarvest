using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator playerAnim;

    [Header("Animation Conditions")]
    public bool isRunning;
    public bool hasLanded;
    public bool isFreeFalling;
    public bool isHanging;


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();

        //Preset values
        hasLanded = false;
        isHanging = false;
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
        bool isJumping = GetComponent<PlayerMovement>().isJumping;
        bool canClimb = GetComponent<PlayerMovement>().canClimb;

        if(isJumping)
        {
            playerAnim.SetBool("jumping", true);
        }
        else 
        {
            playerAnim.SetBool("jumping", false);
        }

        //Landing
        if(hasLanded)
        {
            playerAnim.SetBool("Landing", true);
            playerAnim.SetBool("freefalling", false);
            isFreeFalling = false;
            hasLanded = false;
        }
        else
        {
            playerAnim.SetBool("Landing", false);
        }

        //Free fall
        if(isFreeFalling)
        {
            playerAnim.SetBool("freefalling", true);
        }
        else
        {
            playerAnim.SetBool("freefalling", false);
        }
        
        //Hanging
        if(isHanging)
        {
            playerAnim.SetBool("hanging", true);
        }
        else
        {
            playerAnim.SetBool("hanging", false);
        }

    }
}
