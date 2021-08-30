using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataExtractor : MonoBehaviour
{
    //Variables
    private bool facingRight;

    void Start()
    {
        flip();
    }

    private void flip()
    {
        facingRight = GameObject.Find("Player").GetComponent<PlayerMovement>().facingRight;

        if (!facingRight)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
