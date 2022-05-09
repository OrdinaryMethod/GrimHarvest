using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonitor : MonoBehaviour
{
    private PlayerStats _playerStats;
    private PlayerController _playerController;

    public bool playerIsDead;
    public bool playerIsInsane;

    private float _deathTimer;
    [SerializeField] private float _setDeathTimer;

    public int playerHealth;
    public int playerSanity;

    // Start is called before the first frame update
    private void Awake()
    {
        //Components
        _playerStats = GetComponent<PlayerStats>();
        _playerController = GetComponent<PlayerController>();

        playerIsDead = false;
        playerIsInsane = false;

        _deathTimer = _setDeathTimer;

        //Get initial Stats
        if (_playerStats != null)
        {
            playerHealth = _playerStats.playerHealth;
            playerSanity = _playerStats.playerSanity;
        }
        else
        {
            Debug.LogError("A component on the Player Monitor is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerStats != null)
        {
            CheckPlayerHealth();
        }
        else
        {
            Debug.LogError("A component on the Player Monitor is null");
        }

    }

    private void CheckPlayerHealth()
    {
        if (playerSanity <= 0)
        {
            playerIsInsane = true;

            if(playerIsInsane)
            {
                _deathTimer -= Time.deltaTime;
                _playerController.canMove = false;
                _playerController.rb2d.velocity = new Vector2(0, _playerController.rb2d.velocity.y);

                if (_deathTimer <= 0)
                {
                    playerIsInsane = false;
                    _deathTimer = _setDeathTimer;
                    playerHealth = _playerStats.playerHealth;
                    playerSanity= _playerStats.playerSanity;
                    transform.position = GameObject.Find("PlayerRespawn").transform.position;
                    _playerController.canMove = true;
                }
            }

        }
    }
}
