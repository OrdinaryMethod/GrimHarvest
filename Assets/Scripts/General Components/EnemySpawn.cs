using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    private GameObject Player;

    public LayerMask Enemy;

    private int enemySpawnCount;
    public int enemySpawnLimit;

    private float countDown;
    public float countDownLimit;

    private bool spawnLimitReached;

    [SerializeField] private float DespawnRange;

    void Start()
    {
        enemySpawnCount = enemySpawnLimit;
        countDown = countDownLimit;
    }

    // Update is called once per frame
    void Update()
    {
        GameMaster();
        Timer();
        SpawnEnemyPrefab();
        DespawnEnemyPrefabs();
    }

    private void DespawnEnemyPrefabs()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector2.Distance(gameObject.transform.position, Player.transform.position);

        if(distance > 5)
        {
            Collider2D[] EnemyPrefab = Physics2D.OverlapCircleAll(gameObject.transform.position, DespawnRange, Enemy);
            for (int i = 0; i < EnemyPrefab.Length; i++)
            {
                Destroy(EnemyPrefab[i]);

            }
        }
    }

    private void SpawnEnemyPrefab()
    {
        if (enemySpawnCount > 0 && countDown > 0 && !spawnLimitReached)
        {
            enemySpawnCount--;
            Instantiate(enemyPrefab, gameObject.transform.position, Quaternion.identity);

        }
        else if (enemySpawnCount <= 0)
        {
            countDown -= Time.deltaTime;
        }
    }

    private void Timer()
    {
        if (countDown <= 0)
        {
            enemySpawnCount = enemySpawnLimit;
            countDown = countDownLimit;
        }
    }

    private void GameMaster()
    {
     
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, DespawnRange);
    }

}
