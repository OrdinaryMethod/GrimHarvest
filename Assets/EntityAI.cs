using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityAI : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private NavMeshAgent _agent;
    private Rigidbody2D _rb2d;
    public Transform enemyGFX;

    [Header("Patrol")]
    private GameObject[] _patrolPointObjects;
    [SerializeField] private bool _isPatrolling;
    private int _patrolPointMax;
    private int _patrolPointSelect;
    [SerializeField] private int _selectedPatrolPoint;
    private bool _determineNewPatrolPoint;
    private bool _isResting;

    [Header("Aggro")]
    [SerializeField] private Transform _castPoint;
    [SerializeField] float _aggroRange;



    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _determineNewPatrolPoint = false;
        _isResting = false;

        _rb2d = gameObject.GetComponent<Rigidbody2D>();

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

        if(CanSeePlayer(_aggroRange))
        {
            Debug.Log("Player found");
        }
        else
        {
            Debug.Log("PlayerNotFound");
        }

        //Flip
        if(_rb2d.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(_rb2d.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
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

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDistance = distance;

        Vector2 endPos = _castPoint.position + Vector3.right * distance;

        RaycastHit2D hit = Physics2D.Linecast(_castPoint.position, endPos, 1 << LayerMask.NameToLayer("Default"));

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

            Debug.DrawLine(_castPoint.position, endPos, Color.red);
        }

        return val;
    }
}
