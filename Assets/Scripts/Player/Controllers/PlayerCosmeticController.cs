using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCosmeticController : MonoBehaviour
{
    private PlayerController _playerController;

    [SerializeField] private GameObject[] _playerArmor; 


    // Start is called before the first frame update
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerArmor = GameObject.FindGameObjectsWithTag("PlayerArmor");
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerController.isHidden)
        {
            for(int i = 0; i < _playerArmor.Length; i++)
            {
                _playerArmor[i].GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else
        {
            for (int i = 0; i < _playerArmor.Length; i++)
            {
                _playerArmor[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
