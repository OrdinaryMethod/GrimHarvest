﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingPoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Jumping
        if (collision.gameObject.CompareTag("Surface"))
        {
            GetComponentInParent<PlayerMovement>().isJumping = false;
            StartCoroutine(GroundPlayer());
        }
    }

    IEnumerator GroundPlayer()
    {
        yield return new WaitForSeconds(GetComponentInParent<PlayerMovement>().jumpCooldown);
        GetComponentInParent<PlayerMovement>().isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Jumping
        if (collision.gameObject.CompareTag("Ground"))
        {
            GetComponentInParent<PlayerMovement>().isGrounded = false;
        }
    }
}
