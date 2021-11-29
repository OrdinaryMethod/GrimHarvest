using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveHaunter : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _rb2d;
    [SerializeField] private Transform[] _spawnPoints;
    private Transform _lastSpawnPoint;

    private int _maxSpawnValue;
    [SerializeField] private float _setSpawnCycleTime;
    private float _spawnCycleTime;

    [SerializeField] private float _speed;
    [SerializeField] private float _minCountDownValue;
    [SerializeField] private float _maxCountDownValue;
    private float _countDownValue;
    private bool _countDownReady;

    //State variables
    public bool findingSpawnLocation;
    public bool huntingPlayer;
    public bool returningToSpawnLocation;






    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player");

        //State variables
        findingSpawnLocation = true;
        huntingPlayer = false;
        returningToSpawnLocation = false;

        //Timer variables
        _countDownReady = false;

        Physics2D.IgnoreLayerCollision(8, 11);

        if (_spawnPoints != null)
        {
            foreach (Transform t in _spawnPoints)
            {
                _maxSpawnValue++;
            }
        }

        //Trigger Hunt
        StartCoroutine(TriggerHunt());    
    }

    // Update is called once per frame
    void Update()
    {
        DetermineSpawnPoint();
        HuntPlayer();
        ReturnToPosition();
    }

    void DetermineSpawnPoint()
    {
        if(findingSpawnLocation && !huntingPlayer && !returningToSpawnLocation) //Begin finding spawn point
        {
            if (_maxSpawnValue > 0)
            {
                if(_countDownValue >= 0)
                {
                    _countDownValue -= Time.deltaTime; //Timer until hunt starts

                    _spawnCycleTime -= Time.deltaTime; //Timer for spawn point cycling

                    if (_spawnCycleTime <= 0)
                    {
                        int selectedSpawnPoint = Random.Range(0, _maxSpawnValue);

                        transform.position = _spawnPoints[selectedSpawnPoint].position; //Position to move to

                        _lastSpawnPoint = _spawnPoints[selectedSpawnPoint]; //Position to return if hunt failed

                        _spawnCycleTime = _setSpawnCycleTime; //Repeate until timer hits 0
                    }
                }
                else //Let the hunt begin
                {
                    findingSpawnLocation = false;
                    huntingPlayer = true;
                }
                
            }
        }    
    }

    void HuntPlayer()
    {
        //Check if player is hiding
        if(_player.GetComponent<PlayerController>().isHidden)
        {
            huntingPlayer = false;
        }
        

        if(huntingPlayer && !findingSpawnLocation)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime); //Find and kill the twat
        }  
        else if(!huntingPlayer && !findingSpawnLocation)
        {
            returningToSpawnLocation = true;
        }
    }

    void ReturnToPosition()
    {
        if(!huntingPlayer && !findingSpawnLocation && returningToSpawnLocation)
        {
            transform.position = Vector2.MoveTowards(transform.position, _lastSpawnPoint.position, _speed * Time.deltaTime);

            Debug.Log(_lastSpawnPoint.position);

            if(transform.position == _lastSpawnPoint.position)
            {
                findingSpawnLocation = true;
                returningToSpawnLocation = false;
                _countDownValue = Random.Range(_minCountDownValue, _maxCountDownValue); //Reset countdown to hunt timer
            }
        }
    }

    IEnumerator TriggerHunt()
    {
        _countDownValue = Random.Range(_minCountDownValue, _maxCountDownValue); //Determine initial timer

        yield return 0;
    }
}
