using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityAI : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private NavMeshAgent _agent;
    private CircleCollider2D _cc2d;

    //Patrol
    private GameObject[] _patrolPointObjects;
    [SerializeField] private bool _isPatrolling;
    private int _patrolPointMax;
    private int _patrolPointSelect;
    [SerializeField] private int _selectedPatrolPoint;
    private bool _determineNewPatrolPoint;
    private bool _isResting;



    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _determineNewPatrolPoint = false;
        _isResting = false;

        _cc2d = gameObject.GetComponent<CircleCollider2D>();

        _patrolPointObjects = GameObject.FindGameObjectsWithTag("EnemyPatrolPoint"); 

        _patrolPointMax = _patrolPointObjects.Length;
        _patrolPointSelect = Random.Range(0, _patrolPointMax);
        _selectedPatrolPoint = _patrolPointSelect;

    }

    // Update is called once per frame
    void Update()
    {
        if(!_isPatrolling)
        {
            _agent.SetDestination(_target.position);
            _agent.speed = 30f;

        }
        else
        {
         
                _agent.SetDestination(_patrolPointObjects[_selectedPatrolPoint].transform.position);
            
        
            _agent.speed = 20f;
        }

        DetermineNewPatrolPoint();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyPatrolPoint" && collision.gameObject == _patrolPointObjects[_selectedPatrolPoint])
        {
            _determineNewPatrolPoint = true;
        }
        
    }

    private void DetermineNewPatrolPoint()
    {
 
            if (_determineNewPatrolPoint)
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
