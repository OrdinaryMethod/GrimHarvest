using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameMaster _gameMaster;
    [SerializeField] private List<GameObject> _minorEnemies;
    private int _currentSpawnCount;

    private float _spawnCooldown;

    // Start is called before the first frame update
    void Start()
    {
        _currentSpawnCount = 0;
        _gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        if(_gameMaster != null)
        {
            _spawnCooldown = _gameMaster.setSpawnCooldown;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameMaster != null)
        {
            if(_gameMaster.hordeActive)
            {
                SpawnEnemy();
            }            
        }
        else
        {
            Debug.Log(gameObject.name + " is searching for game master...");
            _gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        }
        
    }

    private void SpawnEnemy()
    {
        if(_spawnCooldown > 0)
        {
            _spawnCooldown -= Time.deltaTime;
        }
        else
        {
            if (_currentSpawnCount < _gameMaster.maxHordeSpawn)
            {
                int enemySelect = Random.Range(0, _minorEnemies.Count);
                _currentSpawnCount++;
                GameObject enemySpawn = Instantiate(_minorEnemies[enemySelect], transform.position, Quaternion.identity);
                enemySpawn.name = enemySpawn.name + "_" + _currentSpawnCount + "_" + gameObject.name;
                enemySpawn.GetComponent<Stats_Enemy>().isHorde = true;
                _spawnCooldown = _gameMaster.setSpawnCooldown;
            }
        }
    }
}
