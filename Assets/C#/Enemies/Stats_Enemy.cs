using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stats_Enemy : MonoBehaviour
{
    public float enemyHealth;
    public float enemyDamage;

    void Update()
    {
        if(enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
