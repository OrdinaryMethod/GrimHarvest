using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text _geigerCounterMeter;
    [SerializeField] private Text _healthMeter;

    [Header("Game Objects")]
    private GeigerCounter _geigerCounter;
    private PlayerMonitor _playerMonitor;

    [Header("Variables")]
    [Range(0.0f, 999.0f)]
    [SerializeField] private float _geigerCounterMax;

    // Start is called before the first frame update
    void Start()
    {
        _geigerCounter = GameObject.Find("Player").GetComponent<GeigerCounter>();
        _playerMonitor = GameObject.Find("Player").GetComponent<PlayerMonitor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_geigerCounter != null || _playerMonitor != null)
        {
            GeigerCounter();
            PlayerHealth();
        }
        else
        {
            Debug.LogError("A UI Element is null.");
        }
    }

    private void GeigerCounter()
    {    
        if(_geigerCounter.anomalyStrength > 0 && _geigerCounter.anomalyStrength <= _geigerCounterMax)
        {
            _geigerCounterMeter.text = Mathf.RoundToInt(_geigerCounter.anomalyStrength) + " rem";
        }
        else if(_geigerCounter.anomalyStrength > _geigerCounterMax)
        {
            _geigerCounterMeter.text = "WARNING";
        }
        else
        {
            _geigerCounterMeter.text = 0 + " rem";
        } 

    }

    private void PlayerHealth()
    {
        _healthMeter.text = _playerMonitor.playerHealth + " units";

        if(_playerMonitor.playerHealth <= 0)
        {
            _healthMeter.text = "Git Gud";
        }
    }
}
