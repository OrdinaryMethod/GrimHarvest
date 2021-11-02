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
    public int bulletAmmo;
    public int dataExtractorAmmo;

    [Header("Shooting Variables")]
    [SerializeField] private float setFireRate;
    [SerializeField] private float bulletSpeed;
    private float fireRate;

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

    //Keybinds
    private KeyCode shootKey;
    private KeyCode extractDataKey;
    private KeyCode meleeAttack;

    // Update is called once per frame
    void Update()
    {
        GetKeyBinds();
        Shoot();
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

    private void Shoot()
    {
        if (fireRate <= 0)
        {
            if (Input.GetKey(shootKey) && bulletAmmo > 0)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, gameObject.transform.rotation);

                Rigidbody2D bulletRb2d;
                bulletRb2d = bullet.GetComponent<Rigidbody2D>();

                if(facingRight)
                {
                    bulletRb2d.velocity = bulletRb2d.GetRelativeVector(Vector2.right * bulletSpeed);
                }
                else
                {
                    bulletRb2d.velocity = bulletRb2d.GetRelativeVector(Vector2.left * bulletSpeed);
                }

                bulletAmmo--;

                Destroy(bullet, 2);              
            }
            fireRate = setFireRate;
        }
        else
        {
            fireRate -= Time.deltaTime;
        }  
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
