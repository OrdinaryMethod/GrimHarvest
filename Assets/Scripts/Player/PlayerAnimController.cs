using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator playerAnim;

    [Header("Animation Conditions")]
    public bool isRunning;
    public bool hasLanded;


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();

        //Preset values
        hasLanded = false;
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
            hasLanded = false;
            //StartCoroutine(StopLandingAnim());
        }
        else
        {
            playerAnim.SetBool("Landing", false);
        }

    }
}
