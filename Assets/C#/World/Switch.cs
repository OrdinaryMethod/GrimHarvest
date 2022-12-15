using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool switchOn;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        switchOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(switchOn)
        {
            _anim.SetBool("SwitchOn", true);
        }
        else
        {
            _anim.SetBool("SwitchOn", false);
        }
    }
}
