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
    void FixedUpdate()
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
            GetComponentInParent<PlayerAnimController>().isHanging = true;
            ledgeCollider = collision.gameObject.GetComponent<Collider2D>(); //Get Collider to ignore   

            GetComponentInParent<PlayerMovement>().climbPoints.Add(new Vector2(collision.gameObject.GetComponent<Ledge>().climpPoint1.position.x, collision.gameObject.GetComponent<Ledge>().climpPoint1.position.y));
            GetComponentInParent<PlayerMovement>().climbPoints.Add(new Vector2(collision.gameObject.GetComponent<Ledge>().climpPoint2.position.x, collision.gameObject.GetComponent<Ledge>().climpPoint2.position.y));
            GetComponentInParent<PlayerMovement>().climbPoints.Add(new Vector2(collision.gameObject.GetComponent<Ledge>().climpPoint3.position.x, collision.gameObject.GetComponent<Ledge>().climpPoint3.position.y));

            Transform newPos = collision.gameObject.GetComponent<Ledge>().LandingPos;
            StartCoroutine(PlayerToLandingPos(newPos));
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
            GetComponentInParent<PlayerAnimController>().isHanging = false;
            StartCoroutine(ResetColliderIgnore());
        }
    }

    IEnumerator PlayerToLandingPos(Transform newPos)
    {
        GameObject.Find("Player").transform.position = newPos.position; //Had to do it this way for whatever reason
        yield return null;
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
