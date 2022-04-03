using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text _geigerCounterMeter;
    [SerializeField] private Text _healthMeter;
    [SerializeField] private Text _statusTextDisplay;
    [SerializeField] private Text _ZoneTextDisplay;
    [SerializeField] private Text _subZoneTextDisplay;

    [Header("Game Objects")]
    private GeigerCounter _geigerCounter;
    private PlayerMonitor _playerMonitor;

    [Header("Scriptable Objects")]
    [SerializeField] private DungeonMasterData _dungeonMasterData;

    [Header("Variables")]
    [Range(0.0f, 999.0f)]
    [SerializeField] private float _geigerCounterMax;

    [Range(1.0f, 10.0f)]
    [SerializeField] private float _setZoneTextTimer;
    private float _zoneTextTimer;

    // Start is called before the first frame update
    void Start()
    {
        _geigerCounter = GameObject.Find("Player").GetComponent<GeigerCounter>();
        _playerMonitor = GameObject.Find("Player").GetComponent<PlayerMonitor>();

        _ZoneTextDisplay.enabled = true;
        _zoneTextTimer = _setZoneTextTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(_geigerCounter != null || _playerMonitor != null || _statusTextDisplay != null || _dungeonMasterData != null || _ZoneTextDisplay != null)
        {
            GeigerCounter();
            PlayerHealth();
            StatusText();
            ZoneText();
        }
        else
        {
            Debug.LogError("A UI Element or object is null.");
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

    private void StatusText()
    {    
         _statusTextDisplay.text = "Sane.";
    }

    private void ZoneText()
    {
        if(_dungeonMasterData.dungeonStarted)
        {
            _ZoneTextDisplay.text = _dungeonMasterData.dungeonName;
            _subZoneTextDisplay.text = "Floor " + _dungeonMasterData.currentLevel.ToString();
        }
        else
        {
            _ZoneTextDisplay.text = "Over World";
            _subZoneTextDisplay.text = "";
        }

        if(_zoneTextTimer > 0f)
        {
            _ZoneTextDisplay.enabled = true;
            _subZoneTextDisplay.enabled = true;
            _zoneTextTimer -= Time.deltaTime;
        }
        else
        {
            _ZoneTextDisplay.enabled = false;
            _subZoneTextDisplay.enabled = false;
        }
        
    }
}
