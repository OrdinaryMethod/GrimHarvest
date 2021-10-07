using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingPoint : MonoBehaviour
{
    public bool landed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Jumping
        if (collision.gameObject.name == "Tilemap_Ground")
        {
            GetComponentInParent<PlayerMovement>().animState = "landing";
            GetComponentInParent<PlayerMovement>().isGrounded = true;
            landed = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Jumping
        if (collision.gameObject.name == "Tilemap_Ground")
        {
            GetComponentInParent<PlayerMovement>().isGrounded = false;
            landed = false;
        }
    }
}
