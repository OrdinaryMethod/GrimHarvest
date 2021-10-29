using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonitor : MonoBehaviour
{
    PlayerStats playerStats;

    public int playerHealth;

    // Start is called before the first frame update
    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>(); //Define
        playerHealth = playerStats.playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //if(playerHealth <= 0)
        //{
        //    Destroy(gameObject);
        //}
        Debug.Log(playerHealth);
    }
}
