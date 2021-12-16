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

    [Header("Misc")]
    private bool _facingRight;



    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _determineNewPatrolPoint = false;
        _isResting = false;
        _facingRight = true;

        _rb2d = gameObject.GetComponent<Rigidbody2D>();

        _patrolPointObjects = GameObject.FindGameObjectsWithTag("EnemyPatrolPoint"); 

        _patrolPointMax = _patrolPointObjects.Length;
        _patrolPointSelect = Random.Range(0, _patrolPointMax);
        _selectedPatrolPoint = _patrolPointSelect;
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
        }
    }

    private void DetermineNewPatrolPoint()
    {
        if (!_isPatrolling)
        {
            _agent.SetDestination(_target.position);
            _agent.speed = 30f;

        }
        else
        {

            _agent.SetDestination(_patrolPointObjects[_selectedPatrolPoint].transform.position);


            _agent.speed = 20f;
        }

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

    private void HuntPlayer()
    {
        if (CanSeePlayer(_aggroRange))
        {
            _isPatrolling = false;
        }
    }

    private void ScanForPlayer()
    {

    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        Vector2 endPos;

        if (_facingRight)
        {
            endPos = _castPoint.position + Vector3.right * distance;
        }
        else
        {
            endPos = _castPoint.position + Vector3.left * distance;
        }
         

        RaycastHit2D hit = Physics2D.Raycast(_castPoint.position, endPos);

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

    void Flip()
    {
        // Switch the way the player is labelled as facing
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
