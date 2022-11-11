using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CorpseFly : MonoBehaviour
{
    public string enemyState;

    private Stats_Enemy stats_enemy;

    [Range(0.0f, 50.0f)][SerializeField] private float _patrolSpeed;
    [Range(0.0f, 50.0f)][SerializeField] private float _chaseSpeed;

    [SerializeField] private GameObject _player;

    [SerializeField] private Transform _target;
    private NavMeshAgent _agent;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private bool _determinPatrolPoint;
    [SerializeField] private Transform _patrolTarget;
    [SerializeField] private int _previousSelectPoint;
    private bool _detectPatrolCollision;
    private bool _facingRight;
    public bool isHorde;

    [HideInInspector] const string isPatrollingEnum = "IsPatrolling";
    [HideInInspector] const string isAttackingEnum = "IsAttacking";

    // Start is called before the first frame update
    void Awake()
    {      
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("Player");
        stats_enemy = gameObject.GetComponent<Stats_Enemy>();
        _agent.updateRotation = false;
        isHorde = false;
        _agent.updateUpAxis = false;
        _determinPatrolPoint = true;
        _detectPatrolCollision = true;
        _facingRight = true;

        enemyState = isPatrollingEnum;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks
        if(_player == null)
        {
            Debug.Log(gameObject.name + " is searching for player...");
            _player = GameObject.Find("Player");
        }

        Patrol();
        Attack();
        ManageState();

        if(isHorde)
        {
            _agent.speed = stats_enemy.enemySpeed;
            _target = GameObject.Find("Player").transform;
        }

        //Agent
        if (_target != null)
        {
            _agent.SetDestination(_target.position);
        }

        //Cosmetic
        if (_agent.velocity.x >= 0.01f && !_facingRight)
        {
            Flip();
        }
        else if (_agent.velocity.x <= -0.01f && _facingRight)
        {
            Flip();
        }
    }

    private void ManageState()
    {
        if(!isHorde)
        {
            switch (enemyState)
            {
                case isPatrollingEnum:
                    _agent.speed = _patrolSpeed;
                    if (_patrolTarget != null)
                    {
                        _target = _patrolTarget;
                    }
                    break;
                case isAttackingEnum:
                    _agent.speed = _chaseSpeed;
                    _target = GameObject.Find("Player").transform;
                    break;
            }
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Attack()
    {
        Vector2 distance = new Vector2(gameObject.transform.position.x - _player.transform.position.x, gameObject.transform.position.y - _player.transform.position.y);

        if (distance.x < 40 && distance.y < 40)
        {
            enemyState = "IsAttacking";
        }
    }

    private void Patrol()
    {      
        if(!isHorde && _patrolPoints.Length <= 0)
        {
            if (_determinPatrolPoint)
            {
                int _selectPoint = Random.Range(0, _patrolPoints.Length);

                if (_selectPoint != _previousSelectPoint)
                {
                    _previousSelectPoint = _selectPoint;
                    _determinPatrolPoint = false;
                    _patrolTarget = _patrolPoints[_selectPoint].transform;
                }
            }
        }    
    }
    IEnumerator ResetPatrolCollision()
    {
        yield return new WaitForSeconds(2f);
        _detectPatrolCollision = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EntityPatrolPoint" && enemyState == isPatrollingEnum && _detectPatrolCollision)
        {
            _detectPatrolCollision = false;
            _determinPatrolPoint = true;
            StartCoroutine(ResetPatrolCollision());
        }

        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponentInParent<PlayerMonitor>().playerHealth -= stats_enemy.enemyDamage;
                break;
        }

    }
}
