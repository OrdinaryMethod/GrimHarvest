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

        Physics2D.IgnoreLayerCollision(10, 11, true);
    }

    // Update is called once per frame
    void Update()
    {
        //Get object components
        playerMovement = GetComponentInParent<PlayerMovement>();

        GrabLedge();
    }
    private void GrabLedge()
    {
        if (playerMovement.grabbingLedge)
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;

            //Climb up
            if (Input.GetKey(KeyCode.Space) && GetComponentInParent<PlayerMovement>().canClimb)
            {
                
            }
        }
        else
        {
            rb2d.constraints = RigidbodyConstraints2D.None;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Ledges
        if (collision.gameObject.CompareTag("Ledge"))
        {
            StartCoroutine(PreventFallOnClimb());
            playerMovement.grabbingLedge = true;
            playerMovement.canMove = false;
            playerMovement.isGrounded = false;
            ledgeCollider = collision.gameObject.GetComponent<Collider2D>(); //Get Collider to ignore   

            GetComponentInParent<PlayerMovement>().climbPoints.Add(new Vector2(collision.gameObject.GetComponent<Ledge>().climpPoint1.position.x, collision.gameObject.GetComponent<Ledge>().climpPoint1.position.y));
            GetComponentInParent<PlayerMovement>().climbPoints.Add(new Vector2(collision.gameObject.GetComponent<Ledge>().climpPoint2.position.x, collision.gameObject.GetComponent<Ledge>().climpPoint2.position.y));
            GetComponentInParent<PlayerMovement>().climbPoints.Add(new Vector2(collision.gameObject.GetComponent<Ledge>().climpPoint3.position.x, collision.gameObject.GetComponent<Ledge>().climpPoint3.position.y));
        }
        else
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Ledges
        if (collision.gameObject.CompareTag("Ledge"))
        {
            playerMovement.grabbingLedge = false;
            playerMovement.canMove = true;
            playerMovement.isGrounded = false;
            playerMovement.canClimb = false;
            StartCoroutine(ResetColliderIgnore());
        }
    }

    //Give time to register grab before allowing to jump in order to prevent falling off cliff
    IEnumerator PreventFallOnClimb()
    {
        yield return new WaitForSeconds(0.2f);
        playerMovement.canClimb = true;
    }

    //Reset Ledge collider ignore
    IEnumerator ResetColliderIgnore()
    {
        yield return new WaitForSeconds(0.1f);
        Physics2D.IgnoreCollision(ledgeCollider, GetComponent<Collider2D>(), false);
    }
}
