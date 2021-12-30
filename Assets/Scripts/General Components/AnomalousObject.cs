using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalousObject : MonoBehaviour
{
    [Header("Settings")]
    public bool IsAnomalous;

    [Range(0.0f, 999.0f)]
    public float anomalyStrength;

    public float distanceToAnomaly;

    void Update()
    {
        distanceToAnomaly = Vector2.Distance(transform.position,GameObject.Find("Player").transform.position);
    }
}
