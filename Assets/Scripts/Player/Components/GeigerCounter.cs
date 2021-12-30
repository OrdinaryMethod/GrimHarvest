using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GeigerCounter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _counterPos;
    [SerializeField] private LayerMask _whatIsAnomaly;

    [Header("Settings")]
    [SerializeField] private float _counterRange;
    public float averageAnomalyStrength;
    public float distanceToAnomaly;

    public List<string> anomalyNames;
    public List<float> anomalyStrengths;


    // Update is called once per frame
    void Update()
    {
        Collider2D[] anomalyCollider = Physics2D.OverlapCircleAll(_counterPos.position, _counterRange, _whatIsAnomaly);
        for (int i = 0; i < anomalyCollider.Length; i++)
        {
            AnomalousObject _anomaly;

            if(anomalyCollider[i].GetComponent<AnomalousObject>() != null)
            {
                _anomaly = anomalyCollider[i].GetComponent<AnomalousObject>();

                if (_anomaly.IsAnomalous)
                {
                    //anomalyStrength = _anomaly.anomalyStrength;           
                    //distanceToAnomaly = _anomaly.distanceToAnomaly;

                    if(!anomalyNames.Contains(anomalyCollider[i].name))
                    {
                        anomalyNames.Add(anomalyCollider[i].name);
                        anomalyStrengths.Add(_anomaly.anomalyStrength);
                    }               
                }
            }
        }

        //Remove anomalies that are not detected from lists
        if(anomalyNames.Count > anomalyCollider.Length)
        {
            anomalyNames.Clear();
            anomalyStrengths.Clear();
        }

        //Get Average anomaly strength near player
        averageAnomalyStrength = (anomalyStrengths.Sum(x => x) / anomalyCollider.Length);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_counterPos.position, _counterRange);
    }
}
