using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    //Variables
    public GameObject bulletPrefab;

    private Vector3 firePoint;

    public float aimSpeed; //Aim speed
    [SerializeField] private bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        firePoint = GetComponentInChildren<Transform>().position;

        if(Input.GetKey(KeyCode.Mouse0))
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint, gameObject.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(25,25);
        }
        
    }

    private void AimDirection()
    {
        float angle;

        float angleUpRight = -315;
        float angleUpLeft = 135;

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
                angle = 180;
            }

        }
        

    }
}
