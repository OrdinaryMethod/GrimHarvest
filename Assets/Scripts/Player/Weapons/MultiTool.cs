using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    //Variables
    public GameObject bulletPrefab;

    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject fireDirection;

    private Vector2 direction;

    public float aimSpeed; //Aim speed
    [SerializeField] private bool facingRight;

    private float angle;

    private float FireRate;
    public float SetFireRate;

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
                direction = Camera.main.ScreenToWorldPoint(fireDirection.transform.position) - transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * 20;
                Destroy(bullet, 1);              
            }
            FireRate = SetFireRate;
        }
        else
        {
            FireRate -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, gameObject.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(firePoint.transform.position.x, firePoint.transform.position.y) * 10;
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
