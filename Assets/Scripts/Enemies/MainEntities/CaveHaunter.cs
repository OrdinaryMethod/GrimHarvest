using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveHaunter : MonoBehaviour
{
    private GameObject player;

    private Rigidbody2D rb2d;
    [SerializeField] private float speed;
    


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");

        Physics2D.IgnoreLayerCollision(8, 11);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
