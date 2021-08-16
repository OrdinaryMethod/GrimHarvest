using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    //Player Components
    Keybinds keybinds;
    PlayerStats playerStats;

    public Transform InteractionPosition;
    public LayerMask Enemy;

    //Player Stats
    private int damage;
    private int knockbackBonus;

    //External combat stats
    private float SwingSpeed;
    public float SetSwingSpeed;
    public float AttackRange;

    //Keybinds
    private KeyCode attack;

    void Update()
    {
        GetKeyBinds();
        GetStats();
        CombatControls();
    }

    private void GetKeyBinds()
    {
        keybinds = GetComponent<Keybinds>(); //Define 

        //Assign
        attack = keybinds.attack;
    }

    private void GetStats()
    {
        playerStats = GetComponent<PlayerStats>(); //Define

        //Assign
        damage = playerStats.damage;
        knockbackBonus = playerStats.knockbackBonus;
    }

    private void CombatControls()
    {
        if (SwingSpeed <= 0)
        {
            if(Input.GetKey(attack)) //Basic attack
            {
                Collider2D[] Enemies = Physics2D.OverlapCircleAll(InteractionPosition.position, AttackRange, Enemy);
                for (int i = 0; i < Enemies.Length; i++)
                {
                    if (Enemies[i].tag == "Enemy")
                    {
                        //Deal Damage
                        int enemyHealth = Enemies[i].GetComponent<EnemyMonitor>().enemyHealth;
                        Enemies[i].GetComponent<EnemyMonitor>().enemyHealth = enemyHealth - damage;

                        //Knockback
                        Vector2 playerPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                        Vector2 enemyPos = new Vector2(Enemies[i].GetComponent<Transform>().position.x, Enemies[i].GetComponent<Transform>().position.y);

                        Vector2 knockBack = (enemyPos - playerPos) * knockbackBonus; //knockback modifier
                       
                        Enemies[i].GetComponent<Rigidbody2D>().AddForce(knockBack / 10);
                    }
                    
                }
                
            }
            SwingSpeed = SetSwingSpeed;

        }
        else
        {
            SwingSpeed -= Time.deltaTime;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(InteractionPosition.position, AttackRange);
    }
}
