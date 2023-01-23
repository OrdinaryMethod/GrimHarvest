using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    //Variables
    private Keybinds _keybinds;
    private PlayerStats _playerStats;
    private PlayerController _playerController;
    private PlayerMonitor _playerMonitor;
    private AudioController_Player _audio;
    private CameraController _cameraController;
    [SerializeField] private GameObject _laserPointer;

    [Header("Objects")]
    [SerializeField] private GameObject _firePoint;
    [SerializeField] private GameObject _redDotPoint;
    public GameObject redDotPrefab; 

    [Header("General Variables")]
    [SerializeField] private bool _facingRight;

    [Header("Shooting Variables")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LineRenderer _aimingLineRenderer;
    private int _shootingDamage;
    public bool lineRendererActive;

    [Header("Melee Variables")]
    [HideInInspector] public bool isMelee;
    public Transform attackPos;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsBarrier;
    public LayerMask whatIsSwitch;
    [Range(0.0f, 25.0f)]
    [SerializeField] private float _attackRange;
    [Range(0.0f, 100.0f)]
    [SerializeField] private float _meleeDamage;

    [Header("Enemy AI")]
    public GameObject noiseSourcePrefab;
    public Transform noiseLocation;

    [Header("Animation")]
    public bool isShooting;

    public float sniperShotCooldown;
    public float setSniperShotCooldown;
    [SerializeField] private bool _sniperShotFired;

    //Keybinds
    private KeyCode _shootKey;
    private KeyCode _meleeAttack;

    void Start()
    {
        _sniperShotFired = false;
        sniperShotCooldown = setSniperShotCooldown;  
        _cameraController = GameObject.Find("Camera").GetComponent<CameraController>();
        _laserPointer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _playerStats = GetComponent<PlayerStats>();
        _playerController = GetComponent<PlayerController>();
        _playerMonitor = GetComponent<PlayerMonitor>();
        _audio = GetComponent<AudioController_Player>();

        

        if (_playerStats != null || _playerController != null)
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
        
        if (Input.GetKeyDown(_shootKey) && !_playerMonitor.playerIsInsane && !_playerMonitor.playerIsDead)
        {
            if(!_playerController.isTouchingFront && !_playerController.isHidden)
            {
                if (_sniperShotFired == false && _playerController.isAiming)
                {
                    _sniperShotFired = true;
                    StartCoroutine(Shoot());
                }
                
            }  
        }

        SniperShotCooldown();

        if(_playerController.isAiming)
        {
            
            _aimingLineRenderer.enabled = true;
            
            _aimingLineRenderer.enabled = false;
            
            _laserPointer.SetActive(true);
            RedDot();
        }
        else
        {
            _aimingLineRenderer.enabled = false;
            _laserPointer.SetActive(false);
        }
    }

    private void SniperShotCooldown()
    {
        //Reset sniper cooldown
        if (_sniperShotFired)
        {
            sniperShotCooldown -= Time.deltaTime;
        }

        if (sniperShotCooldown <= 0)
        {
            _sniperShotFired = false;
            sniperShotCooldown = setSniperShotCooldown;
        }
    }

    private void GetKeyBinds()
    {
        _keybinds = GetComponentInParent<Keybinds>(); //Define 

        //Assign
        _shootKey = _keybinds.shoot;
        _meleeAttack = _keybinds.meleeAttack;
    }

    private void RedDot()
    {
        RaycastHit2D hitInfo;      

        if (_facingRight)
        {
            hitInfo = Physics2D.Raycast(_redDotPoint.transform.position, _redDotPoint.transform.right);

            if (hitInfo)
            {            
                //Spawn line
                _aimingLineRenderer.SetPosition(0, _redDotPoint.transform.position);
                _aimingLineRenderer.SetPosition(1, hitInfo.point);
            }
        }
        else
        {
            hitInfo = Physics2D.Raycast(_redDotPoint.transform.position, -_redDotPoint.transform.right);


            if (hitInfo)
            {
                //Spawn line
                _aimingLineRenderer.SetPosition(1, _redDotPoint.transform.position);
                _aimingLineRenderer.SetPosition(0, hitInfo.point);
            }           
        }

        if(_playerController.isAiming)
        {
            GameObject redDot = Instantiate(redDotPrefab, hitInfo.point, Quaternion.identity);
            StartCoroutine(DestroyRedDot(redDot));
        }

        if(hitInfo)
        {
            _playerController.flashLightDistance = hitInfo.distance;
        }

        //Ignore certain trigger colliders
        if(hitInfo)
        {
            if(hitInfo.transform.gameObject.tag == "Switch" || hitInfo.transform.gameObject.tag == "EntityPatrolPoint" || hitInfo.transform.gameObject.tag == "NoiseSource")
            {
                Physics2D.queriesHitTriggers = false;
            }
            else
            {
                Physics2D.queriesHitTriggers = true;
            }
        }
        
    }

    IEnumerator DestroyRedDot(GameObject redDot)
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(redDot);
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        
        _audio.sniperShot = true;
        

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

            //Enemy
            Stats_Enemy enemyStats = hitInfo.transform.GetComponent<Stats_Enemy>();
            if(enemyStats)
            {
                enemyStats.enemyHealth -= _shootingDamage;
            }
        }

        _lineRenderer.enabled = true;
        lineRendererActive = true;

        //StartCoroutine(_cameraController.Shake(0.05f));

        yield return new WaitForSeconds(0.02f);

        _lineRenderer.enabled = false;
        lineRendererActive = false;
    }

    private void MeleeAttack()
    {
        if(Input.GetKeyDown(_meleeAttack) && !_playerController.isTouchingFront)
        {
            isMelee = true;

            //basic enemy
            Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(attackPos.position, _attackRange, whatIsEnemy);
            for (int i = 0; i < enemyCollider.Length; i++)
            {
                Debug.Log("You strike an enemy");
                enemyCollider[i].GetComponentInParent<Stats_Enemy>().enemyHealth -= _meleeDamage;
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

            //Switch
            Collider2D[] switchCollider = Physics2D.OverlapCircleAll(attackPos.position, _attackRange, whatIsSwitch);
            for (int i = 0; i < switchCollider.Length; i++)
            {
                if (!switchCollider[i].GetComponentInParent<Switch>().switchOn)
                {
                    switchCollider[i].GetComponentInParent<Switch>().switchOn = true;
                }
                else
                {
                    switchCollider[i].GetComponentInParent<Switch>().switchOn = false;
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
