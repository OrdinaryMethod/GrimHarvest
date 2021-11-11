using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidBomb : MonoBehaviour
{
    [SerializeField] private Transform explosionPos;
    [SerializeField] private float bombTimer;
    [SerializeField] private float explosionRange;
    [SerializeField] private float damage;

    //Layer Masks
    public LayerMask whatIsEnemy;
    public LayerMask whatIsBarrier;
    public LayerMask whatIsTrigger;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(6, 3);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, bombTimer);
        bombTimer -= Time.deltaTime;
        if(bombTimer <= 0)
        {
            //enemy
            Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(explosionPos.position, explosionRange, whatIsEnemy);
            for (int i = 0; i < enemyCollider.Length; i++)
            {
                Debug.Log("Droid bomb hits enemy!");
            }

            //Barrier
            Collider2D[] barrierCollider = Physics2D.OverlapCircleAll(explosionPos.position, explosionRange, whatIsBarrier);
            for (int i = 0; i < barrierCollider.Length; i++)
            {
                if (barrierCollider[i].GetComponentInParent<Barrier>().canMelee || barrierCollider[i].GetComponentInParent<Barrier>().canShoot)
                {
                    Debug.Log("Explosion hits barrier!");
                    barrierCollider[i].GetComponentInParent<Barrier>().barrierHealth -= damage;
                }
            }

            //Trigger
            Collider2D[] triggerCollider = Physics2D.OverlapCircleAll(explosionPos.position, explosionRange, whatIsTrigger);
            for (int i = 0; i < triggerCollider.Length; i++)
            {
                
            }

            //Destroy(gameObject);
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(explosionPos.position, explosionRange);
    }
}
