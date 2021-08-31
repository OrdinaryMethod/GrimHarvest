using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    //Variables
    Keybinds keybinds;

    public GameObject bulletPrefab;
    public GameObject DataExtractorPrefab;

    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject fireDirection;

    [SerializeField] private bool facingRight;
    
    private float angle;

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

    //Keybinds
    private KeyCode shootKey;
    private KeyCode extractDataKey;

    // Update is called once per frame
    void Update()
    {
        GetKeyBinds();
        AimDirection();
        Shoot();
        ExtractData();

        //Get parent values
        facingRight = GetComponentInParent<PlayerMovement>().facingRight;     
    }

    private void GetKeyBinds()
    {
        keybinds = GetComponentInParent<Keybinds>(); //Define 

        //Assign
        shootKey = keybinds.shoot;
        extractDataKey = keybinds.dataExtractor;
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

    private void AimDirection()
    {
        float angleUpRight = -315;
        float angleUpLeft = 315;

        if (Input.GetKey(KeyCode.E))
        {
            if (facingRight)
            {
                angle = angleUpRight;
            }
            else
            {
                angle = angleUpLeft;
            }
        }
        else
        {
            if (facingRight)
            {
                angle = -360;
            }
            else
            {
                angle = 360;
            }         
        }
        transform.eulerAngles = Vector3.forward * angle;
    }
}
