using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPoint : MonoBehaviour
{
    //Variables
    PlayerMovement playerMovement;

    [HideInInspector] public Collider2D ledgeCollider;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Get object components
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.CompareTag("Surface"))
    //    {
    //        playerMovement.grabbingLedge = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    //Ledges
    //    if (collision.gameObject.CompareTag("Surface"))
    //    {
    //        playerMovement.grabbingLedge = false;
    //    }
    //}
}
