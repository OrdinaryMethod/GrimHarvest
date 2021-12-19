using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseSource : MonoBehaviour
{
    [Header("Settings")]
    [HideInInspector] public bool resetNoise;
    public bool noiseExpired;
    public float setTimeToExpire;
    public float timeToExpire;
   
    // Start is called before the first frame update
    void Awake()
    {
        resetNoise = false;
        noiseExpired = false;
        timeToExpire = setTimeToExpire;
    }

    // Update is called once per frame
    void Update()
    {
        if(resetNoise)
        {
            resetNoise = false;
            noiseExpired = false;
            timeToExpire = setTimeToExpire;
        }

        if(timeToExpire > 0)
        {
            timeToExpire -= Time.deltaTime;
            if (timeToExpire <= 0)
            {
                noiseExpired = true;
            }
        }
    }
}
