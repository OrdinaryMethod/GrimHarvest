using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameMaster _gameMaster;
    [SerializeField] private List<GameObject> _minorEnemies;
    [SerializeField] private List<string> _activeEnemies;

    [Range(1, 15)] private float _setEnemyDamage;
    [Range(1, 15)] private float _setEnemyHealth;

    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    private int _currentSpawnCount;
    private int _totalSpawnCount;

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
                CheckForSpawnedEnemies();
            } 
            else
            {
                _currentSpawnCount = 0;
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
                GameObject enemySpawn = Instantiate(_minorEnemies[enemySelect], transform.position, Quaternion.identity);
                enemySpawn.name = enemySpawn.name + "_" + _totalSpawnCount + "_" + gameObject.name;
                _activeEnemies.Add(enemySpawn.name);

                _currentSpawnCount++;
                _totalSpawnCount++;

                enemySpawn.GetComponent<Stats_Enemy>().isHorde = true;
                enemySpawn.GetComponent<Stats_Enemy>().enemySpeed = Random.Range(_minSpeed, _maxSpeed);
                _spawnCooldown = _gameMaster.setSpawnCooldown;
            }
        }
    }

    private void CheckForSpawnedEnemies()
    {
        #pragma warning disable
        if (_activeEnemies != null || _activeEnemies.Count > 0)
        {
            foreach(string enemyName in _activeEnemies)
            {     
                GameObject spawnedEnemy = GameObject.Find(enemyName);
                if (spawnedEnemy == null)
                {
                    _currentSpawnCount--;
                    break;
                    _activeEnemies.Remove(enemyName);
                }
            }
        }
        #pragma warning restore
    }
}
