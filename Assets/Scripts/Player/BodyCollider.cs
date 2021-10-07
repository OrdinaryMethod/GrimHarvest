using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollider : MonoBehaviour
{
    //Variables
    PlayerMovement playerMovement;

    // Update is called once per frame
    void Update()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Surface") || collision.gameObject.CompareTag("Wall"))
        {
            if (!playerMovement.isGrounded)
            {
                playerMovement.grabbingLedge = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Surface") || collision.gameObject.CompareTag("Wall"))
        {
            if (!playerMovement.isGrounded)
            {
                playerMovement.grabbingLedge = false;
            }
        }
    }
}
