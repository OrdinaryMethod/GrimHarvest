using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonitor : MonoBehaviour
{
    //Variables
    public int enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponent<EnemyStats>().enemyHealth; //Get initial value
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
