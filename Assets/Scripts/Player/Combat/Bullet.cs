using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Variables
    private Transform bulletRotation;
    public float bulletDamage;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(6,3);

        //bulletRotation = GameObject.Find("LArm").GetComponent<Transform>();

        //transform.eulerAngles = new Vector3(0,0, bulletRotation.eulerAngles.z);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
