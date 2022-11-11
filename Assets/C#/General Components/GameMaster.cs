using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private GameObject _player;

    public int maxHordeSpawn;
    public bool hordeActive;
    public float setSpawnCooldown;
    private bool _despawn;
    public float setPlayerRespawnTime;

    void Start()
    {
        _player = GameObject.Find("Player");

        _despawn = false;
    }

    //left off here
    void Update()
    {
        DestroyEnemies();
    }

    private void DestroyEnemies()
    {       
        if(_player.GetComponent<PlayerMonitor>().playerIsDead)
        {
            _despawn = true;
        }

        if(_despawn)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");

            if(_player.GetComponent<PlayerMonitor>().playerRespawnTime <= 0)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                    hordeActive = false;
                }
                else
                {
                    _despawn = false;
                    _player.GetComponent<PlayerMonitor>().playerIsDead = false;
                    _player.GetComponent<PlayerMonitor>().playerRespawnTime = setPlayerRespawnTime;
                }
            }       
        }
        
    }
}
