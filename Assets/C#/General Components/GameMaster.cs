using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        ResetScene();
    }

    private void ResetScene()
    {
        if(_player.GetComponent<PlayerMonitor>().playerIsDead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }           
    }

  
}
