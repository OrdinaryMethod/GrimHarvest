using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    //Variables
    public float barrierHealth;
    public bool canShoot;
    public bool canMelee;

    void Update()
    {
        BarrierHealthCheck();    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(canShoot && canMelee)
        {
           
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
