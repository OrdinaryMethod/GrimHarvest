using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text _geigerCounterMeter;

    [Header("Game Objects")]
    private GeigerCounter _geigerCounter;

    [Header("Variables")]
    private float _meterCount;

    // Start is called before the first frame update
    void Start()
    {
        _geigerCounter = GameObject.Find("Player").GetComponent<GeigerCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        GeigerCounter();
    }

    private void GeigerCounter()
    {
        _meterCount = _geigerCounter.distanceToAnomaly;

        if(_geigerCounter.averageAnomalyStrength > 0)
        {
            //_meterCount = _geigerCounter.strongestAnomalyValue / Mathf.RoundToInt(_geigerCounter.distanceToAnomaly * 2);
        }
        else
        {
            _meterCount = 0;
        }

        if(_meterCount > 999)
        {
           // _meterCount = _geigerCounter.strongestAnomalyValue;
        }

        _geigerCounterMeter.text = Mathf.RoundToInt(_meterCount) + " rem";

        
    }
}
