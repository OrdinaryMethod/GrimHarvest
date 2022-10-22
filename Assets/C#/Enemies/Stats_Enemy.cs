using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stats_Enemy : MonoBehaviour
{
    private CorpseFly _corpseFly;

    public float enemyHealth;
    public bool isHorde;

    // Start is called before the first frame update
    void Awake()
    {
        _corpseFly = gameObject.GetComponent<CorpseFly>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(_corpseFly != null)
        {
            _corpseFly.isHorde = true;
        }

        if(enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
