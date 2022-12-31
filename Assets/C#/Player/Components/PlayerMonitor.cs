using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonitor : MonoBehaviour
{
    private PlayerStats _playerStats;
    private PlayerController _playerController;
    private GameObject _gameMaster;

    public bool playerIsDead;
    public bool playerIsInsane;

    private float _deathTimer;
    [SerializeField] private float _setDeathTimer;

    public float playerHealth;
    public int playerSanity;
    public float playerRespawnTime;
    public bool isInvincible;
    [SerializeField] private float _setInvincibleTime;
    private float _invincibleTime;

    // Start is called before the first frame update
    private void Awake()
    {
        //Components
        _playerStats = GetComponent<PlayerStats>();
        _playerController = GetComponent<PlayerController>();
        _gameMaster = GameObject.Find("GameMaster");

        playerIsDead = false;
        playerIsInsane = false;
        isInvincible = false;

        _invincibleTime = _setInvincibleTime;

        _deathTimer = _setDeathTimer;
        playerRespawnTime = _gameMaster.GetComponent<GameMaster>().setPlayerRespawnTime;

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
            TurnInvincible();
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
                    //transform.position = GameObject.Find("PlayerRespawn").transform.position;
                    _playerController.canMove = true;
                }
            }

        }

        if(playerHealth <= 0)
        {
            playerIsDead = true;
        }

        if(playerIsDead)
        {
            playerRespawnTime -= Time.deltaTime;
            _playerController.canMove = false;

            if(playerRespawnTime <= 0)
            {
                //gameObject.transform.position = new Vector3(GameObject.Find("Respawn").transform.position.x, GameObject.Find("Respawn").transform.position.y, 0);
                playerHealth = _playerStats.playerHealth;               
            }          
        }
    }

    private void TurnInvincible()
    {
        if (isInvincible)
        {
            _invincibleTime -= Time.deltaTime;
            if(_invincibleTime <= 0)
            {                           
                isInvincible = false;
                _invincibleTime = _setInvincibleTime;
            }

        }
    }
}
