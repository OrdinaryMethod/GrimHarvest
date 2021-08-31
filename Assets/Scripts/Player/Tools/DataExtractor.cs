using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataExtractor : MonoBehaviour
{
    //Variables
    private Rigidbody2D rb2d;

    private bool facingRight;
    private bool dataExtractionComplete;

    public float dataExtractionTime;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
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
        rb2d.velocity = new Vector2(0, 0);
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(ExtractTheData());

        //Recollect
        if(collision.gameObject.CompareTag("Player"))
        {
            if(dataExtractionComplete)
            {
                collision.gameObject.GetComponentInChildren<MultiTool>().dataExtractorAmmo++;
                Destroy(gameObject);
            }          
        }
    }

    IEnumerator ExtractTheData()
    {
        Debug.Log("Extracting data...");
        yield return new WaitForSeconds(dataExtractionTime);
        dataExtractionComplete = true;
        Debug.Log("Data gained");
    }
}
