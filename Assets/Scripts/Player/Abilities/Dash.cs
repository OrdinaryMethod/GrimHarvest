using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float setCooldown;
    private float cooldown;

    private bool cooldownActive;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = setCooldown;
        cooldownActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKey(KeyCode.E))
        //{
        //    if(!cooldownActive)
        //    {
        //        cooldownActive = true;
        //        //Get mouse position
        //        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        //        Vector2 characterToMouse = worldPosition - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        //        gameObject.GetComponent<Rigidbody2D>().AddForce(characterToMouse / 100);
        //    }            
        //}
        
        //if(cooldownActive)
        //{
        //    cooldown -= Time.deltaTime;

        //    if(cooldown <= 0)
        //    {
        //        cooldownActive = false;
        //        cooldown = setCooldown;
        //    }
        //}
    }
}
