using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalousObject : MonoBehaviour
{
    [Header("Settings")]
    public bool IsAnomalous;

    [Range(0.0f, 200.0f)]
    public float anomalyStrength;
}
