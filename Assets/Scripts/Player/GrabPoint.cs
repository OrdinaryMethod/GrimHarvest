using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPoint : MonoBehaviour
{
    //Variables

    private Collider2D ledgeCollider;

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
        GrabLedge();
    }
    private void GrabLedge()
    {
        if (GetComponentInParent<PlayerMovement>().grabbingLedge)
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;

            //Climb up
            if (Input.GetKey(KeyCode.Space) && GetComponentInParent<PlayerMovement>().canClimb)
            {
                GetComponentInParent<PlayerMovement>().grabbingLedge = false;
                rb2d.constraints = RigidbodyConstraints2D.None;
                rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

                //Get colliders to ignore
                Physics2D.IgnoreCollision(ledgeCollider, GetComponent<Collider2D>(), true);
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
            GetComponentInParent<PlayerMovement>().grabbingLedge = true;
            GetComponentInParent<PlayerMovement>().canMove = false;
            GetComponentInParent<PlayerMovement>().isGrounded = false;
            ledgeCollider = collision.gameObject.GetComponent<Collider2D>(); //Get Collider to ignore
            StartCoroutine(PreventFallOnClimb());
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
            GetComponentInParent<PlayerMovement>().grabbingLedge = false;
            GetComponentInParent<PlayerMovement>().canMove = true;
            GetComponentInParent<PlayerMovement>().isGrounded = false;
            GetComponentInParent<PlayerMovement>().canClimb = false;
            StartCoroutine(ResetColliderIgnore());
        }
    }

    //Give time to register grab before allowing to jump in order to prevent falling off cliff
    IEnumerator PreventFallOnClimb()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponentInParent<PlayerMovement>().canClimb = true;
        Debug.Log("Kablam");
    }

    //Reset Ledge collider ignore
    IEnumerator ResetColliderIgnore()
    {
        yield return new WaitForSeconds(0.1f);
        Physics2D.IgnoreCollision(ledgeCollider, GetComponent<Collider2D>(), false);
    }
}
