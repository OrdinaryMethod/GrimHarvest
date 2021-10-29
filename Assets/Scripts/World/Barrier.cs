using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    //Variables
    [SerializeField] private float barrierHealth;
    [SerializeField] private bool canShoot;
    [SerializeField] private bool canMelee;

    void Update()
    {
        BarrierHealthCheck();    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(canShoot && canMelee)
        {
            if (collision.gameObject.tag == "PlayerBullet")
            {
                barrierHealth = barrierHealth - (collision.gameObject.GetComponent<Bullet>().bulletDamage);
            }
        }
        else if(!canShoot && canMelee)
        {
            if (collision.gameObject.tag == "PlayerMelee")
            {
                
            }
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
