using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityAI : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _patrolSpeed;
    [SerializeField] private float _chaseSpeed;
    private NavMeshAgent _agent;

    [Header("Patrol")]
    [SerializeField] private bool _isPatrolling;
    [SerializeField] private int _selectedPatrolPoint;
    private GameObject[] _patrolPointObjects;
    private int _patrolPointMax;
    private int _patrolPointSelect;
    private bool _determineNewPatrolPoint;
    private bool _isResting;
    [SerializeField] private float _setTimeToRest;
    private float _timeToRest;

    [Header("Aggro")]
    [SerializeField] private Transform _castPoint;
    [SerializeField] float _aggroRange;

    [Header("Misc")]
    private bool _facingRight;
    private PlayerController _playerController;



    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _determineNewPatrolPoint = false;
        _facingRight = true;
        _isResting = false;

        _patrolPointObjects = GameObject.FindGameObjectsWithTag("EnemyPatrolPoint");
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        _patrolPointMax = _patrolPointObjects.Length;
        _patrolPointSelect = Random.Range(0, _patrolPointMax);
        _selectedPatrolPoint = _patrolPointSelect;
        _timeToRest = _setTimeToRest;
    }

    // Update is called once per frame
    void Update()
    {
        DetermineNewPatrolPoint();
        HuntPlayer();

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyPatrolPoint" && collision.gameObject == _patrolPointObjects[_selectedPatrolPoint])
        {
            _determineNewPatrolPoint = true;
            _isResting = true;
        }
    }

    private void DetermineNewPatrolPoint()
    {
        if (!_isPatrolling)
        {
            _agent.SetDestination(_target.position);
            _agent.speed = _chaseSpeed;

            if(_playerController.isHidden)
            {
                _isPatrolling = true;
            }
        }
        else
        {
            _agent.SetDestination(_patrolPointObjects[_selectedPatrolPoint].transform.position);
            _agent.speed = _patrolSpeed;
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
                    Debug.Log(_timeToRest);
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
            _isPatrolling = false;
        }
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        Vector2 endPos;

        endPos = _target.position - _castPoint.position;

        RaycastHit2D hit = Physics2D.Raycast(_castPoint.position, endPos, distance);

        if(hit.collider != null)
        {
            if(hit.collider.gameObject.CompareTag("Player") && !_playerController.isHidden)
            {
                val = true;
            }
            else
            {
                val = false;
            }

            //Debug.DrawRay(_castPoint.position, endPos, Color.red);
        }

        return val;
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
