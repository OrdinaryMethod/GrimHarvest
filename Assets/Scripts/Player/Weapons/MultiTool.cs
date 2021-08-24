using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    //Variables
    public GameObject bulletPrefab;

    private Vector3 firePoint;

    public float aimSpeed; //Aim speed
    [HideInInspector] public bool facingRight;

    [Header("Aim Angles")]
    private float setAngle = 0;
    private float angle; //Global aim angle

    [SerializeField] private float setAngleUp;
    private float angleUp;

    // Start is called before the first frame update
    void Start()
    {
        //Set values
        angleUp = setAngleUp;
        angle = setAngle;       
    }

    // Update is called once per frame
    void Update()
    {
        AimDirection();
        Shoot();
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
        //Player Movement. Check for horizontal movement
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {    
            if (Input.GetAxisRaw("Horizontal") > 0.5f && !facingRight)
            {
                //If we're moving right but not facing right, flip the sprite and set     facingRight to true.
                Flip();
                facingRight = true;

                //Flip aim angles
                angleUp = setAngleUp;
                angle = setAngle;

            }
            else if (Input.GetAxisRaw("Horizontal") < 0.5f && facingRight)
            {
                //If we're moving left but not facing left, flip the sprite and set facingRight to false.
                Flip();
                facingRight = false;

                //Flip aim angles
                angleUp = setAngleUp + 90;
                angle = setAngle + 180;
                
            }

        }
        
        if(Input.GetKey(KeyCode.E))
        {
            Debug.Log("test");
            angle = angleUp;
            
        }
        else
        {
            if(facingRight)
            {
                angle = 0;
            }
            else
            {
                angle = 180;
            }
            
        }

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, aimSpeed * Time.deltaTime);
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
