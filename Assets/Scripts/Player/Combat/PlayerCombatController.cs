using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    //Variables
    private Keybinds _keybinds;
    private PlayerStats _playerStats;
    private PlayerController _playerController;

    [Header("Objects")]
    [SerializeField] private GameObject _firePoint;

    [Header("General Variables")]
    [SerializeField] private bool _facingRight;

    [Header("Shooting Variables")]
    [SerializeField] private LineRenderer _lineRenderer;
    private int _shootingDamage;

    [Header("Melee Variables")]
    public Transform attackPos;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsBarrier;
    [Range(0.0f, 25.0f)]
    [SerializeField] private float _attackRange;
    [Range(0.0f, 100.0f)]
    [SerializeField] private float _meleeDamage;

    [Header("Enemy AI")]
    public GameObject noiseSourcePrefab;
    public Transform noiseLocation;

    [Header("Animation")]
    public bool isShooting;

    //Keybinds
    private KeyCode _shootKey;
    private KeyCode _meleeAttack;

    // Update is called once per frame
    void Update()
    {
        //Components
        _playerStats = GetComponent<PlayerStats>();
        _playerController = GetComponent<PlayerController>();

        //Variables
        if(_playerStats != null || _playerController != null)
        {
            _facingRight = _playerController.facingRight;
            _shootingDamage = _playerStats.shootingDamage;

            GetKeyBinds();
            MeleeAttack();
        }
        else
        {
            Debug.LogError("A component on the Player Combat script is null.");
        }
        
        if (Input.GetKeyDown(_shootKey))
        {
            StartCoroutine(Shoot());
        }    
    }

    private void GetKeyBinds()
    {
        _keybinds = GetComponentInParent<Keybinds>(); //Define 

        //Assign
        _shootKey = _keybinds.shoot;
        _meleeAttack = _keybinds.meleeAttack;
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        GameObject noiseSource = GameObject.Find("NoiseLocation(Clone)");

        if(noiseSource == null)
        {
            GameObject noiseSourceSpawn = Instantiate(noiseSourcePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            noiseSource.transform.position = transform.position;
            noiseSource.GetComponent<NoiseSource>().resetNoise = true;
        }

        RaycastHit2D hitInfo;

        if (_facingRight)
        {
            hitInfo = Physics2D.Raycast(_firePoint.transform.position, _firePoint.transform.right);

            if (hitInfo)
            {
                //Spawn line
                _lineRenderer.SetPosition(0, _firePoint.transform.position);
                _lineRenderer.SetPosition(1, hitInfo.point);
            }
            else
            {
                //Spawn line
                _lineRenderer.SetPosition(0, _firePoint.transform.position);
                _lineRenderer.SetPosition(1, _firePoint.transform.position + _firePoint.transform.right * 100);
            }
        }
        else
        {
            hitInfo = Physics2D.Raycast(_firePoint.transform.position, -_firePoint.transform.right);
                

            if (hitInfo)
            {                
                //Spawn line
                _lineRenderer.SetPosition(1, _firePoint.transform.position);
                _lineRenderer.SetPosition(0, hitInfo.point);
            }
            else
            {

                //Spawn line
                _lineRenderer.SetPosition(1, _firePoint.transform.position);
                _lineRenderer.SetPosition(0, _firePoint.transform.position - _firePoint.transform.right * 100);
            }
        }

        //Damage Logic
        if(hitInfo)
        {
            //Barrier
            Barrier barrier = hitInfo.transform.GetComponent<Barrier>();
            if(barrier != null)
            {
                if(barrier.canShoot)
                {
                    barrier.barrierHealth -= _shootingDamage;
                }            
            }
        }

        _lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);

        _lineRenderer.enabled = false;
    }

    private void MeleeAttack()
    {
        if(Input.GetKeyDown(_meleeAttack))
        {          
            Debug.Log("You Melee Attack");

            //basic enemy
            Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(attackPos.position, _attackRange, whatIsEnemy);
            for (int i = 0; i < enemyCollider.Length; i++)
            {
                Debug.Log("You strike an enemy");
                //enemyCollider[i].GetComponentInParent<EnemyController>().playerHealth -= damage;
            }

            //barriers
            Collider2D[] barrierCollider = Physics2D.OverlapCircleAll(attackPos.position, _attackRange, whatIsBarrier);
            for (int i = 0; i < barrierCollider.Length; i++)
            {
                if (barrierCollider[i].GetComponentInParent<Barrier>().canMelee)
                {
                    Debug.Log("You strike a barrier");
                    barrierCollider[i].GetComponentInParent<Barrier>().barrierHealth -= _meleeDamage;
                }
            }      
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, _attackRange);
    }
}
