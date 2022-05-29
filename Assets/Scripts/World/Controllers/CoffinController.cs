using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinController : MonoBehaviour
{
    private GameObject _player;
    private Animator _animator;
    [SerializeField] private Transform _hidingPos;
    [SerializeField] private LayerMask _whatIsPlayer;

    [Range(0.0f, 25.0f)][SerializeField] private float _hidingRange;

    // Start is called before the first frame update
    void Awake()
    {
        _player = GameObject.Find("Player");
        _animator = GetComponent<Animator>();

        if ( _player == null || _animator == null)
        {
            Debug.LogError(name + "cannot find player.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] playerCollider = Physics2D.OverlapCircleAll(_hidingPos.position, _hidingRange, _whatIsPlayer);
        for (int i = 0; i < playerCollider.Length; i++)
        {          
            if (Input.GetKey(KeyCode.S) && playerCollider[i].GetComponentInParent<PlayerController>().isGrounded)
            {
                playerCollider[i].GetComponentInParent<PlayerController>().isHidden = true;
                _animator.SetBool("isHiding", true);
            }
            else
            {
                playerCollider[i].GetComponentInParent<PlayerController>().isHidden = false;
                _animator.SetBool("isHiding", false);
            }
            
        }

        //if(playerCollider.Length == 0)
        //{
        //    _player.GetComponent<PlayerController>().isHidden = false;
        //    _animator.SetBool("isHiding", false);
        //}     
    }

    private void OnDrawGizmosSelected()
    {     
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hidingPos.position, _hidingRange);     
    }
}
