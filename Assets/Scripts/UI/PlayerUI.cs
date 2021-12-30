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
    [Range(0.0f, 999.0f)]
    [SerializeField] private float _geigerCounterMax;

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
}
