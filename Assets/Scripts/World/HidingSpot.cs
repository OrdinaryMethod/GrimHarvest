using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public Transform hidingSpotPos;
    public LayerMask whatIsPlayer;
    public float hidingRange;

    // Start is called before the first frame update
    void Start()
    {
        hidingSpotPos = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] playerCollider = Physics2D.OverlapCircleAll(hidingSpotPos.position, hidingRange, whatIsPlayer);
        for (int i = 0; i < playerCollider.Length; i++)
        {
            if(playerCollider[i].GetComponentInParent<PlayerController>().isCrouching)
            {
                playerCollider[i].GetComponentInParent<PlayerController>().isHidden = true;
            }
            else
            {
                playerCollider[i].GetComponentInParent<PlayerController>().isHidden = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hidingSpotPos.position, hidingRange);
    }
}
