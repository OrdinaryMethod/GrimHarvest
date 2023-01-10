using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController_Player : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerCombatController _playerCombatController;
    private WallClimbing _wallClimbing;

    [SerializeField] private GameObject _running;
    [SerializeField] private GameObject _landing;
    [SerializeField] private GameObject _sniperShot;
    [SerializeField] private GameObject _wallSliding;

    public bool sniperShot;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCombatController = GetComponent<PlayerCombatController>();
        _wallClimbing = GetComponent<WallClimbing>();

        sniperShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        StereoPanAdjuster();

        if (_playerController != null) //Main controller
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

        if(_wallClimbing != null)
        {
            //Wall sliding
            if(_wallClimbing.wallSliding)
            {
                _wallSliding.active = true;
            }
            else
            {
                _wallSliding.active = false;
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

    private void StereoPanAdjuster()
    {
        if(_playerController.facingRight)
        {
            _sniperShot.GetComponent<AudioSource>().panStereo = (float)-0.25;
        }
        else
        {
            _sniperShot.GetComponent<AudioSource>().panStereo = (float)0.25;
        }
    }

    private IEnumerator ResetSniperAudio()
    {
        yield return new WaitForSeconds(_playerCombatController.setSniperShotCooldown - 0.1f);
        
        _sniperShot.active = false;
    }
}
