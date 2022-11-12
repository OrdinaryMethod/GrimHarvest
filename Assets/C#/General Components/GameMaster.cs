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
        DestroyEnemies();
        ResetEntity();
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

    private void ResetEntity()
    {
        if(_player.GetComponent<PlayerMonitor>().playerIsDead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //if(_player.GetComponent<PlayerMonitor>().playerIsDead)
        //{
        //    GameObject[] entity = GameObject.FindGameObjectsWithTag("Entity");

        //    foreach (GameObject o in entity)
        //    {
        //        if (o.GetComponent<EntityAI>() != null)
        //        {
        //            if (o.GetComponent<EntityAI>().startPos != null)
        //            {
        //                o.GetComponent<EntityAI>()._entityState = "Patrolling";
        //                o.GetComponent<EntityAI>()._isResting = false;
        //                o.transform.position = new Vector3(o.GetComponent<EntityAI>().startPos.x, o.GetComponent<EntityAI>().startPos.y, o.transform.position.z);
        //            }
        //        }
        //    }
        //}           
    }
}
