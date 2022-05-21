using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCosmeticController : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerCombatController _playerCombatController;
    private PlayerAnimController _playerAnimController;

    [Header("Muzzle Flash")]
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private Sprite[] _muzzleFlashSprite;
    [SerializeField] private bool _isShooting;
    //[SerializeField] private bool 

    [Header("Armor")]
    [SerializeField] private GameObject[] _playerArmor;
    


    // Start is called before the first frame update
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCombatController = GetComponent<PlayerCombatController>();
        _playerAnimController = GetComponent<PlayerAnimController>();

        _playerArmor = GameObject.FindGameObjectsWithTag("PlayerArmor");
    }

    // Update is called once per frame
    void Update()
    {
        //DarkenArmor();
        SpawnMuzzleFlash();
        TurnOffFlashLight();
    }

    private void DarkenArmor()
    {
        if (_playerController.isHidden)
        {
            for (int i = 0; i < _playerArmor.Length; i++)
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

    private void SpawnMuzzleFlash()
    {

        if(_playerCombatController.lineRendererActive)
        {
            int muzzleFlashSpriteSelect = Random.Range(0, _muzzleFlashSprite.Length);
            float muzzleFlashSize = Random.Range(0.3f, 0.8f);
            _muzzleFlash.GetComponent<SpriteRenderer>().sprite = _muzzleFlashSprite[muzzleFlashSpriteSelect];
            _muzzleFlash.transform.localScale = new Vector2(muzzleFlashSize, muzzleFlashSize);
            _muzzleFlash.SetActive(true);
        }
        else
        {
            _muzzleFlash.SetActive(false);
        }
    }

    private void TurnOffFlashLight()
    {
        if (_playerController.isTouchingFront)
        {
            _playerController.playerFlashLight.SetActive(false);
        }
        else
        {
            _playerController.playerFlashLight.SetActive(true);
        }
    }
}
