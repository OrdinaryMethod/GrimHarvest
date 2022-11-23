using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool switchOn;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        switchOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(switchOn)
        {
            anim.SetBool("SwitchOn", true);
        }
    }
}
