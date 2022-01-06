using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityAI : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private string _entityState;

    [Header("Main Settings")]
    [SerializeField] private Transform _target;
    [Range(0.0f, 50.0f)]
    [SerializeField] private float _patrolSpeed;
    [Range(0.0f, 50.0f)]
    [SerializeField] private float _chaseSpeed;
    private NavMeshAgent _agent;

    [Header("Patrol")]
    [SerializeField] private int _selectedPatrolPoint;
    private GameObject[] _patrolPointObjects;
    private int _patrolPointMax;
    private int _patrolPointSelect;
    private bool _determineNewPatrolPoint;
    [SerializeField] private bool _isResting;
    [Range(0.0f, 10.0f)]
    [SerializeField] private float _setTimeToRest;
    private float _timeToRest;

    [Header("Aggro")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _castPoint;
    [Range(0.0f, 100.0f)]
    [SerializeField] private float _aggroRange;
    public Transform PlayerHidingPos;

    [Header("Noise Detection")]
    [SerializeField] private Transform _hearingPos;
    [SerializeField] private LayerMask _whatIsNoise;
    [Range(0.0f, 1000.0f)]
    [SerializeField] private float _hearingRange;
    private Transform _noisePosition;

    [Header("Misc")]
    private bool _facingRight;
    private PlayerController _playerController;
    private float distanceToPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        _entityState = "Patrolling";

        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _determineNewPatrolPoint = false;
        _facingRight = true;
        _isResting = false;

        _patrolPointObjects = GameObject.FindGameObjectsWithTag("EntityPatrolPoint");
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        _patrolPointMax = _patrolPointObjects.Length;
        _patrolPointSelect = Random.Range(0, _patrolPointMax);
        _selectedPatrolPoint = _patrolPointSelect;
        _timeToRest = _setTimeToRest;
    }

    // Update is called once per frame
    void Update()
    {
        if(_patrolPointObjects.Length == 0)
        {
            Debug.LogError("Please setup entity patrol points within this scene.");
        }
        else if(_target == null)
        {
            Debug.LogError("Please assign the entity ai a target in it's inspector.");
        }
        else
        {
            Pathing();
            HuntPlayer();
            ListenForNoise();
            PlayerCantHide();
        }

        //Flip
        if (_agent.velocity.x >= 0.01f && !_facingRight)
        {
            Flip();
        }
        else if (_agent.velocity.x <= -0.01f && _facingRight)
        {
            Flip();
        }
    }

    private void Pathing()
    {
        if (!_isResting && _entityState == "Hunting") //Chase Player
        {
            _agent.SetDestination(_target.position);
            _agent.speed = _chaseSpeed;
        }
        else if (!_isResting && _entityState == "Patrolling") //Patrol
        {
            _agent.SetDestination(_patrolPointObjects[_selectedPatrolPoint].transform.position);
            _agent.speed = _patrolSpeed;
        }
        else if(!_isResting && _entityState == "Investigating") //Persue noise
        {
            if(_noisePosition != null)
            {
                _agent.SetDestination(_noisePosition.position);
            }
            else
            {
                _entityState = "Patrolling";
            }
        }
        else if(!_isResting && _entityState == "Suspicious") //Search for Player
        {
            _agent.SetDestination(_target.position);
            _agent.speed = _chaseSpeed;
        }

        if (_determineNewPatrolPoint)
        {
            if(_isResting)
            {
                _timeToRest -= Time.deltaTime;

                if(_timeToRest <= 0)
                {
                    _isResting = false;
                    _timeToRest = _setTimeToRest;
                }
            }
            else
            {
                if (_patrolPointSelect == _selectedPatrolPoint)
                {
                    _patrolPointSelect = Random.Range(0, _patrolPointMax);
                }
                else
                {
                    _selectedPatrolPoint = _patrolPointSelect;
                    _determineNewPatrolPoint = false;
                }
            }
        }       
    }

    private void HuntPlayer()
    {
        if (CanSeePlayer(_aggroRange))
        {
            if(!_playerController.isHidden)
            {
                _entityState = "Hunting";
                _isResting = false;
            }   
        }
    }

    private void PlayerCantHide()
    {
        if(_entityState == "Hunting" && _playerController.isHidden)
        {
            distanceToPlayer = Vector2.Distance(transform.position, _target.position);

            if (distanceToPlayer <= _aggroRange)
            {
                _agent.SetDestination(_target.position);
                _agent.speed = _chaseSpeed;

                Debug.Log(name + " can still see you...");
            }  
            else if(distanceToPlayer > _aggroRange)
            {
                _entityState = "Suspicious";
            }
        }
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        Vector2 endPos;

        endPos = _target.position - _castPoint.position;

        RaycastHit2D hit = Physics2D.Raycast(_castPoint.position, endPos, distance, _layerMask);

        if(hit.collider != null)
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                val = true;
            }
            else
            {
                val = false;
            }

            Debug.DrawRay(_castPoint.position, endPos, Color.red);
        }

        return val;
    }

    private void ListenForNoise()
    {
        Collider2D[] noiseCollider = Physics2D.OverlapCircleAll(_hearingPos.position, _hearingRange, _whatIsNoise);
        for (int i = 0; i < noiseCollider.Length; i++)
        {
            if(!noiseCollider[i].GetComponent<NoiseSource>().noiseExpired)
            {
                if(_entityState != "Hunting")
                {
                    StartCoroutine(PersueNoise(noiseCollider[i].transform));
                }    
            }
            
        }
    }

    IEnumerator PersueNoise(Transform noisePosition)
    {
        _noisePosition = noisePosition;

        _entityState = "Investigating";

        yield return new WaitForSeconds(0);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (_entityState == "Suspicious" && _playerController.isHidden)
            {
                _entityState = "Patrolling";
                _determineNewPatrolPoint = true;
                _isResting = true;
            }
        }
        else if (collision.gameObject.tag == "EntityPatrolPoint" && collision.gameObject == _patrolPointObjects[_selectedPatrolPoint])
        {
            _determineNewPatrolPoint = true;
            _isResting = true;
        }
        else if (collision.gameObject.tag == "NoiseSource")
        {
            if(_entityState != "Hunting")
            {
                _timeToRest = _setTimeToRest;

                _entityState = "Patrolling";

                _determineNewPatrolPoint = true;
                _isResting = true;
            }
            
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Entity")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hearingPos.position, _hearingRange);
    }
}
