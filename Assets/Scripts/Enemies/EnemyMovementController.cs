using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    GameObject[] nearbyEnemies;

    public Vector2 SpawnPoint;
    private Transform Target;
    private Rigidbody2D rb2d;

    public float distance;
    public float AggroResetDistance;
    public float Speed;

    private void Awake()
    {

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        SpawnPoint = gameObject.transform.position;        
    }

    void Update()
    {
        AggroControl();

        nearbyEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject i in nearbyEnemies)
        {
            Collider2D enemyCollider = i.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(enemyCollider, GetComponent<Collider2D>());
        }      
    }
   

    private void AggroControl()
    {
        //Hunt player
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        //Persue
        if (Target != null)
        {
            distance = Vector2.Distance(SpawnPoint, Target.transform.position); //Calculate distance between spawn point and player
            if(distance <= 10)
            {
                gameObject.transform.position = Vector2.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime); //Aggro

                float knockBackDistance = Vector2.Distance(gameObject.transform.position, Target.transform.position);

                //check for knockback and stop enemy from flying
                if ((rb2d.velocity.x != 0 || rb2d.velocity.y != 0) && knockBackDistance > 2)
                {
                    rb2d.velocity = new Vector2(0, 0);
                }
            }
            else
            {
                gameObject.transform.position = Vector2.MoveTowards(transform.position, SpawnPoint, Speed * Time.deltaTime); //Reset if player too far away
            }          
        }
        else
        {
            gameObject.transform.position = Vector2.MoveTowards(transform.position, SpawnPoint, Speed * Time.deltaTime); //Reset if player dies
        }
    }
}
