using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHazard : MonoBehaviour
{
    private float timeBtwTick;
    public float startTimeBtwTick;

    public Transform hazardPos;
    public LayerMask whatIsPlayer;
    public GameObject trigger;

    public bool active;

    public float hazardRange;
    public int damage;

    void Update()
    {
        if(timeBtwTick <= 0)
        {
            timeBtwTick = startTimeBtwTick;
            Collider2D[] playerCollider = Physics2D.OverlapCircleAll(hazardPos.position, hazardRange, whatIsPlayer);
            for (int i = 0; i < playerCollider.Length; i++)
            {
                playerCollider[i].GetComponentInParent<PlayerMonitor>().playerHealth -= damage;
            }
        }
        else
        {
            timeBtwTick -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hazardPos.position, hazardRange);
    }
}
