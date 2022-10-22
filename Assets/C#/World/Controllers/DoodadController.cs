using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodadController : MonoBehaviour
{
    private CapsuleCollider2D _capsuleCollider;
    private Animator _animator;
    [SerializeField] private Transform _dangerPos;
    [SerializeField] private LayerMask _whatIsPlayer;

    [SerializeField] private bool _isDangerous;
    [SerializeField] private bool _flip;

    [Range(0.0f, 25.0f)]
    [SerializeField] private float _dangerRange;

    private float _dangerCooldown;
    [SerializeField] private float _setDangerCooldown;

    // Start is called before the first frame update
    void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();

        if(_isDangerous && _setDangerCooldown > 0)
        {
            _dangerCooldown = _setDangerCooldown;
        }
        else
        {
            Debug.LogWarning("Dangerous doodad " + name + " has no cooldown time set.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_capsuleCollider != null)
        {
            LowerSanity();
            Flip();
        }
        else
        {
            Debug.LogError(gameObject.name + " does not detect a collider.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _animator.SetBool("Collided", true);
            StartCoroutine(ResetCollisionAnim());
        }
    }

    IEnumerator ResetCollisionAnim()
    {
        yield return new WaitForSeconds(1f);
        _animator.SetBool("Collided", false);
    }

    private void LowerSanity()
    {
        if(_isDangerous)
        {
            _dangerCooldown -= Time.deltaTime;

            if(_dangerCooldown <= 0)
            {
                Collider2D[] playerCollider = Physics2D.OverlapCircleAll(_dangerPos.position, _dangerRange, _whatIsPlayer);
                for (int i = 0; i < playerCollider.Length; i++)
                {
                    playerCollider[i].GetComponentInParent<PlayerMonitor>().playerSanity -= 1;
                }

                _dangerCooldown = _setDangerCooldown;
            }      
        }
    }

    private void Flip()
    {
        if(_flip)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            _flip = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(_isDangerous)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_dangerPos.position, _dangerRange);
        }       
    }
}
