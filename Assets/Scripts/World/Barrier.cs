using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    //Variables
    [SerializeField] private float barrierHealth;

    void Update()
    {
        BarrierHealthCheck();    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Landing
        if (collision.gameObject.tag == "PlayerBullet")
        {
            barrierHealth = barrierHealth - (collision.gameObject.GetComponent<Bullet>().bulletDamage);
        }
    }

    private void BarrierHealthCheck()
    {
        if(barrierHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

}
