using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonitor : MonoBehaviour
{
    private PlayerStats _playerStats;

    public int playerHealth;

    // Start is called before the first frame update
    private void Awake()
    {
        //Components
        _playerStats = GetComponent<PlayerStats>(); 

        //Get initial Stats
        if(_playerStats != null)
        {
            playerHealth = _playerStats.playerHealth;
        }
        else
        {
            Debug.LogError("A component on the Player Monitor is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerStats != null)
        {
            CheckPlayerHealth();
        }
        else
        {
            Debug.LogError("A component on the Player Monitor is null");
        }

    }

    private void CheckPlayerHealth()
    {
        if (playerHealth <= 0)
        {
            Debug.Log("Player has died");
        }
    }
}
