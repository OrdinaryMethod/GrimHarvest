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
    public float anomalyStrength;
    public float distanceToAnomaly;

    [Header("Tracked Variables")]
    public List<string> anomalyNames;
    public List<AnomalousObject> anomalyStrengths; //This list will be used to tell engine to refresh counter lists

    // Update is called once per frame
    void Update()
    {
        Collider2D[] anomalyCollider = Physics2D.OverlapCircleAll(_counterPos.position, _counterRange, _whatIsAnomaly);
        for(int i = 0; i < anomalyCollider.Length; i++)
        {
            AnomalousObject _anomaly;

            if(anomalyCollider[i].GetComponent<AnomalousObject>() != null)
            {
                _anomaly = anomalyCollider[i].GetComponent<AnomalousObject>();

                if(_anomaly.IsAnomalous)
                {
                    if(!anomalyNames.Contains(anomalyCollider[i].name))
                    {
                        anomalyNames.Add(anomalyCollider[i].name);
                        anomalyStrengths.Add(_anomaly);
                    }    

                    if(anomalyStrengths.Count > 0)
                    {
                        anomalyStrengths.Where(w => w.name == _anomaly.name).ToList().ForEach(S => S.anomalyStrength = _anomaly.anomalyStrength);
                    }
                }
            }
        }

        if (anomalyNames.Count > anomalyCollider.Length)
        {
            anomalyNames.Clear();
            anomalyStrengths.Clear();
        }

        if(anomalyCollider.Length == 0)
        {
            distanceToAnomaly = 0;
            anomalyStrength = 0;
        }
        else
        {
            Mathf.RoundToInt(anomalyStrength = anomalyStrengths.Sum(x => x.anomalyStrength));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_counterPos.position, _counterRange);
    }
}
