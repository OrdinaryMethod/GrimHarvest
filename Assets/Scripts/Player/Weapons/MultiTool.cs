using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    //Variables
    public GameObject bulletPrefab;

    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject fireDirection;

    public float SetFireRate;

    [SerializeField] private bool facingRight;
    [SerializeField] private float bulletSpeed;
    private float angle;
    private float FireRate;

    // Update is called once per frame
    void Update()
    {
        AimDirection();
        Shoot();

        //Get parent values
        facingRight = GetComponentInParent<PlayerMovement>().facingRight;     
    }

    private void Shoot()
    {
        if (FireRate <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
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

                Destroy(bullet, 2);              
            }
            FireRate = SetFireRate;
        }
        else
        {
            FireRate -= Time.deltaTime;
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
