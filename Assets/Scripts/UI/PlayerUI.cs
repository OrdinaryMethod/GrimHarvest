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
        _geigerCounterMeter.text = _geigerCounter.anomalyStrength + " rem";
    }
}
