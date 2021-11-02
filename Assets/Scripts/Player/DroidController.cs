using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidController : MonoBehaviour
{
    [SerializeField] private Transform dockPos;
    private PlayerController playerController;

    private bool droidActive;
    private float moveInputX;
    private float moveInputY;


    // Start is called before the first frame update
    void Start()
    {
        
        droidActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerController = GetComponentInParent<PlayerController>();

        if (!droidActive)
        {
            gameObject.transform.position = new Vector3(dockPos.position.x, dockPos.position.y, 0);
        }
        else
        {
            playerController.GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0, 0);
            playerController.animState = "idle";

            Movement();
        }

        playerController.droidActive = droidActive;

        ActivateDroid();
        
    }

    private void ActivateDroid()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(!droidActive)
            {
                if(playerController.isGrounded)
                {
                    droidActive = true;
                }                    
            }
            else
            {
                droidActive = false;
            }      
        }
    }

    private void Movement()
    {
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveInputX, moveInputY) * 10;
    }
}
