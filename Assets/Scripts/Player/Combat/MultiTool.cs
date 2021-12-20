using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    //Variables
    Keybinds keybinds;

    [Header("Prefabs")]
    public GameObject bulletPrefab;
    public GameObject DataExtractorPrefab;

    [Header("Objects")]
    [SerializeField] private GameObject firePoint;

    [Header("General Variables")]
    [SerializeField] private bool facingRight;

    [Header("Ammo")]
    public int dataExtractorAmmo;

    [Header("Shooting Variables")]
    public LineRenderer lineRenderer;
    [SerializeField] private LayerMask _ignoreLayer;
    public int shootingDamage;

    [Header("Data Extractor Variables")]
    [SerializeField] private float setExtractorRate;
    [SerializeField] private float dataExtractorSpeed;
    private float extractorRate;

    [Header("Melee Variables")]
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsBarrier;
    public float attackRange;
    public int damage;

    [Header("Enemy AI")]
    public GameObject noiseSourcePrefab;
    public Transform noiseLocation;

    //Keybinds
    private KeyCode shootKey;
    private KeyCode extractDataKey;
    private KeyCode meleeAttack;

    // Update is called once per frame
    void Update()
    {
        GetKeyBinds();
        if (Input.GetKeyDown(shootKey))
        {
            StartCoroutine(Shoot());
        }          
        ExtractData();
        MeleeAttack();

        //Get parent values
        facingRight = GetComponent<PlayerController>().facingRight;
    }

    private void GetKeyBinds()
    {
        keybinds = GetComponentInParent<Keybinds>(); //Define 

        //Assign
        shootKey = keybinds.shoot;
        extractDataKey = keybinds.dataExtractor;
        meleeAttack = keybinds.meleeAttack;
    }

    private void ExtractData()
    {
        if (extractorRate <= 0)
        {
            if (Input.GetKey(extractDataKey) && dataExtractorAmmo > 0)
            {
                GameObject dataExtractor = Instantiate(DataExtractorPrefab, firePoint.transform.position, gameObject.transform.rotation);

                Rigidbody2D dataExtractorRb2d;
                dataExtractorRb2d = dataExtractor.GetComponent<Rigidbody2D>();

                if (facingRight)
                {
                    dataExtractorRb2d.velocity = dataExtractorRb2d.GetRelativeVector(Vector2.right * dataExtractorSpeed);
                }
                else
                {
                    dataExtractorRb2d.velocity = dataExtractorRb2d.GetRelativeVector(Vector2.left * dataExtractorSpeed);
                }

                dataExtractorAmmo--;
            }
            extractorRate = setExtractorRate;
        }
        else
        {
            extractorRate -= Time.deltaTime;
        }
    }

    IEnumerator Shoot()
    {
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

        if (facingRight)
        {
            hitInfo = Physics2D.Raycast(firePoint.transform.position, firePoint.transform.right, _ignoreLayer);

            if (hitInfo)
            {
                Debug.Log(hitInfo.transform.name);

                //Spawn line
                lineRenderer.SetPosition(0, firePoint.transform.position);
                lineRenderer.SetPosition(1, hitInfo.point);
            }
            else
            {
                //Spawn line
                lineRenderer.SetPosition(0, firePoint.transform.position);
                lineRenderer.SetPosition(1, firePoint.transform.position + firePoint.transform.right * 100);
            }
        }
        else
        {
            hitInfo = Physics2D.Raycast(firePoint.transform.position, -firePoint.transform.right, _ignoreLayer);
                

            if (hitInfo)
            {
                Debug.Log(hitInfo.transform.name);
                    
                //Spawn line
                lineRenderer.SetPosition(1, firePoint.transform.position);
                lineRenderer.SetPosition(0, hitInfo.point);
            }
            else
            {

                //Spawn line
                lineRenderer.SetPosition(1, firePoint.transform.position);
                lineRenderer.SetPosition(0, firePoint.transform.position - firePoint.transform.right * 100);
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
                    barrier.barrierHealth -= shootingDamage;
                }            
            }
            
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;
    }

    private void MeleeAttack()
    {
        if(Input.GetKey(meleeAttack))
        {
            if (timeBtwAttack <= 0)
            {
                Debug.Log("You Melee Attack");
                timeBtwAttack = startTimeBtwAttack;

                //basic enemy
                Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
                for (int i = 0; i < enemyCollider.Length; i++)
                {
                    Debug.Log("You strike an enemy");
                    //enemyCollider[i].GetComponentInParent<EnemyController>().playerHealth -= damage;
                }

                //barriers
                Collider2D[] barrierCollider = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsBarrier);
                for (int i = 0; i < barrierCollider.Length; i++)
                {
                    if (barrierCollider[i].GetComponentInParent<Barrier>().canMelee)
                    {
                        Debug.Log("You strike a barrier");
                        barrierCollider[i].GetComponentInParent<Barrier>().barrierHealth -= damage;
                    }
                }
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
