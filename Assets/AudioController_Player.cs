using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController_Player : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerCombatController _playerCombatController;

    [SerializeField] private GameObject _running;
    [SerializeField] private GameObject _landing;
    [SerializeField] private GameObject _sniperShot;

    public bool sniperShot;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCombatController = GetComponent<PlayerCombatController>();

        sniperShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerController != null) //Main controller
        {
            //Running
            if (_playerController.isRunning)
            {
                _running.active = true;


            }
            else
            {
                _running.active = false;
            }

            //Landing
            if(_playerController.isGrounded)
            {
                _landing.active = true;
            }
            else
            {
                _landing.active = false;
            }          
        } 

        if(_playerCombatController != null && _playerController != null) //Combat Controller
        {
            //Sniper shot
            if(sniperShot)
            {
                _sniperShot.active = true;
                sniperShot = false;
                StartCoroutine(ResetSniperAudio());
            }
        }
    }

    private IEnumerator ResetSniperAudio()
    {
        yield return new WaitForSeconds(_playerCombatController.setSniperShotCooldown);
        
        _sniperShot.active = false;
    }
}
