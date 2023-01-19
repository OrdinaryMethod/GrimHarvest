using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DJ : MonoBehaviour
{
    public GameObject temporaryAmbience;

    private float tempAmbienceCooldown;
    public float setTempAmbienceCooldown;

    public static DJ instance;

    // Start is called before the first frame update
    void Awake()
    {
        tempAmbienceCooldown = setTempAmbienceCooldown;

        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        if(temporaryAmbience != null)
        {
            if(tempAmbienceCooldown <= 0)
            {
                temporaryAmbience.GetComponent<AudioSource>().mute = !temporaryAmbience.GetComponent<AudioSource>().mute;
                tempAmbienceCooldown = setTempAmbienceCooldown;
            }
            else
            {
                tempAmbienceCooldown -= Time.deltaTime;
            }
            
        }
    }
}
