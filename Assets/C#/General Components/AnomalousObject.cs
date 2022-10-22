using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalousObject : MonoBehaviour
{
    [Header("Settings")]
    public bool IsAnomalous;
    [Range(0.0f, 1000.0f)]
    public float setAnomalyStrength;
    public float anomalyStrength;
    public float distanceToAnomaly;

    void Update()
    {
 
        distanceToAnomaly = Vector2.Distance(transform.position,GameObject.Find("Player").transform.position);
        Mathf.RoundToInt(anomalyStrength = setAnomalyStrength / distanceToAnomaly);

        if(anomalyStrength > setAnomalyStrength)
        {
            anomalyStrength = setAnomalyStrength;
        }
    }
}
