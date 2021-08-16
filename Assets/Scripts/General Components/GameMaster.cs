using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public int maxEnemyLimit;
    public bool spawnLimitReached;

    // Update is called once per frame
    void Update()
    {
        ControlEnemySpawns();
    }

    private void ControlEnemySpawns()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount >= maxEnemyLimit)
        {
            spawnLimitReached = true;
        }
        else
        {
            spawnLimitReached = false;
        }
    }
}
