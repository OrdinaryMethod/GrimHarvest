using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject Text;
    [SerializeField] private GameObject _startDistanceObject;
    [SerializeField] private GameObject _endDistanceObject;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _distance;
    public bool isHidden;

    // Start is called before the first frame update
    void Start()
    {
        isHidden = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Text == null || _startDistanceObject == null || _endDistanceObject == null || _minDistance <= 0)
        {
            Debug.LogError(gameObject.name + " is missing a tooltip setting!");
        }
        else
        {
            float _distance = Vector3.Distance(_startDistanceObject.transform.position, _endDistanceObject.transform.position);

            if (_distance >= _minDistance)
            {
                isHidden = true;
            }
            else
            {
                isHidden = false;
            }

            if (isHidden)
            {
                Text.active = false;
            }
            else
            {
                Text.active = true;
            }
        }       
    }
}
