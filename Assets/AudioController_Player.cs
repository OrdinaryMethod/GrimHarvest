using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController_Player : MonoBehaviour
{
    private PlayerController _playerController;

    [SerializeField] private GameObject _running;
  

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();      
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerController != null)
        {
            if (_playerController.isRunning)
            {
                _running.active = true;


            }
            else
            {
                _running.active = false;
            }
        } 
    }
}
