using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeigerCounter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _counterPos;
    [SerializeField] private LayerMask _whatIsAnomaly;

    [Header("Settings")]
    [SerializeField] private float _counterRange;


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

                if(_anomaly.IsAnomalous)
                {
                    Debug.Log("Anomaly Detected! Strength level: " + _anomaly.anomalyStrength);
                }
            }

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_counterPos.position, _counterRange);
    }
}
